namespace proj

open UnityEngine
open UnityEngine.InputSystem

type Magnetude = float32

type Movement() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let inputActions: InputActionAsset = null

    [<SerializeField>]
    let mutable movementSpeed = 50f

    let mutable moveAction: InputAction = null

    member _.OnEnable() = moveAction.Enable()
    member _.OnDisable() = moveAction.Disable()

    member _.Awake() =
        let playerMap = inputActions.FindActionMap("Player", true)
        moveAction <- playerMap.FindAction("Move", true)

    member this.Update() =
        let movement = moveAction.ReadValue<Vector2>()

        Time.deltaTime * movementSpeed * Vector3(movement.x, movement.y, 0f)
        |> this.transform.Translate
