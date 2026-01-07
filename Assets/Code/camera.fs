namespace proj

open UnityEngine
open UnityEngine.InputSystem

[<RequireComponent(typeof<Camera>)>]
type MoveCamera() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let mutable playerToFollow: GameObject = null

    [<SerializeField>]
    let mutable movementSpeed = 100f

    [<SerializeField>]
    let mutable rotationSpeed = 10f

    [<SerializeField>]
    let mutable offset = Vector3.zero

    [<SerializeField>]
    let inputActions: InputActionAsset = null

    let mutable cameraAction: InputAction = null

    let mutable yaw = 0f
    let mutable pitch = 0f

    // peut-être prendre cette valeur d'un GameObject qui a le move.fs script?
    let shouldLockPlayerRotation = false

    member _.Awake() =
        let playerMap = inputActions.FindActionMap("Player", true)
        cameraAction <- playerMap.FindAction("LookAround", true)

    member _.OnEnable() = cameraAction.Enable()
    member _.OnDisable() = cameraAction.Disable()

    member _.Start() =
        Cursor.lockState <- CursorLockMode.Locked
        Cursor.visible <- false

        ()

    member this.Update() =
        if playerToFollow <> null then
            // gère la rotation de la caméra
            let mouseDelta = cameraAction.ReadValue<Vector2>()

            yaw <- yaw + mouseDelta.x * rotationSpeed * Time.deltaTime
            pitch <- Mathf.Clamp(pitch - mouseDelta.y * rotationSpeed * Time.deltaTime, -89f, 89f)

            let rotation = Quaternion.Euler(pitch, yaw, 0f)
            let rotatedOffset = rotation * offset

            let targetPosition = playerToFollow.transform.position + rotatedOffset

            this.transform.position <-
                Vector3.Lerp(this.transform.position, targetPosition, movementSpeed * Time.deltaTime)

            this.transform.rotation <- Quaternion.LookRotation(-rotatedOffset.normalized)


            // gère la rotation de l'objet suivi

            if shouldLockPlayerRotation then
                let angles = this.transform.rotation.eulerAngles
                playerToFollow.transform.rotation <- Quaternion.Euler(0f, angles.y, 0f)


        ()
