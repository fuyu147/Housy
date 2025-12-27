namespace proj

open UnityEngine

[<RequireComponent(typeof<Camera>)>]
type MoveCamera() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let movementSpeed = 100

    [<SerializeField>]
    let rotationSpeed = 10

    [<SerializeField>]
    let offset = Vector3.zero

    member _.Start() = ()
    member _.Update() = ()
