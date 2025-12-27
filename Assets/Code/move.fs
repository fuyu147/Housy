namespace proj

open UnityEngine
open UnityEngine.InputSystem

type Magnetude = float32

type Movement() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let inputActions: InputActionAsset = null

    let mutable move: InputAction = null

    member _.OnEnable() = move.Enable()
    member _.OnDisable() = move.Disable()

    // this is a useless comment
    member _.Awake() =
        let playerMap = inputActions.FindActionMap("PlayerMovement", true)

        move <- playerMap.FindAction("Move", true)
        ()

    member this.Update() =
        let mousePosition = Mouse.current.position.ReadValue()
        Debug.Log <| sprintf "mouse position : <%f, %f>" mousePosition.x mousePosition.y

        let movement = move.ReadValue<Vector2>()

        Time.deltaTime * Vector3(movement.x, movement.y, 0f) |> this.transform.Translate
