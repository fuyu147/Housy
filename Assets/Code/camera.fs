namespace proj

open UnityEngine

[<RequireComponent(typeof<Camera>)>]
type MoveCamera() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let movementSpeed = 100

    [<SerializeField>]
    let rotationSpeed = 10

    member _.Start () = ()
    member _.Update () = ()
