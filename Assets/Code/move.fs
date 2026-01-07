namespace proj

open UnityEngine
open UnityEngine.InputSystem

type Magnetude = float32

module Printing =
    let print = Debug.Log

open Printing

[<RequireQualifiedAccess>]
type Direction =
    | Up
    | Down
    | Left
    | Right

[<RequireQualifiedAccess>]
type MovementAction =
    | Walking of Direction
    | Running of Direction
    | WallJump of Direction

[<RequireComponent(typeof<Rigidbody>)>]
type Movement() =
    inherit MonoBehaviour()

    [<Header("Camera Stuff")>]
    [<SerializeField>]
    let mutable cameraGO: Camera = null

    [<Header("Input action asset")>]
    [<SerializeField>]
    let inputActions: InputActionAsset = null

    let mutable moveAction: InputAction = null

    [<Header("Movement settings")>]
    [<SerializeField>]
    let mutable movementSpeed = 50f

    [<SerializeField>]
    let mutable jumpForce = 100f

    [<SerializeField>]
    let mutable maxOffgroundJump = 1

    [<Header("Layers")>]
    [<SerializeField>]
    let mutable terrainLayer = 0

    [<SerializeField>]
    let mutable wallLayer = 0

    [<SerializeField>]
    let mutable ceilingLayer = 0


    let mutable currentOffgroundJumpsLeft = maxOffgroundJump

    // private stuff

    let mutable timeSinceDirectionChange = 0f

    let mutable isHoldingJump = false

    let mutable isOnGround = false

    let mutable jumpAction: InputAction = null

    let mutable rg: Rigidbody = null

    // member(public) stuff

    member _.OnEnable() =
        moveAction.Enable()
        jumpAction.Enable()

    member this.OnDisable() =
        moveAction.Disable()
        jumpAction.Disable()
        jumpAction.remove_started (fun ctx -> this.OnJump(ctx))
        jumpAction.remove_canceled (fun ctx -> this.OnJump(ctx))

    member this.Awake() =
        let playerMap = inputActions.FindActionMap("Player", true)
        moveAction <- playerMap.FindAction("Move", true)
        jumpAction <- playerMap.FindAction("Jump", true)
        jumpAction.add_started (fun ctx -> this.OnJump(ctx))
        jumpAction.add_canceled (fun ctx -> this.OnJump(ctx))

    member this.Start() = rg <- this.GetComponent<Rigidbody>()

    member this.OnCollisionEnter(collision: Collision) =
        if collision.gameObject.layer = LayerMask.NameToLayer("Terrain") then
            isOnGround <- true

    member this.OnCollisionExit(collision: Collision) =
        if collision.gameObject.layer = LayerMask.NameToLayer("Terrain") then
            isOnGround <- false

    member this.IsOnWall = false

    member this.CanJump() =
        isOnGround || this.IsOnWall || currentOffgroundJumpsLeft > 0

    member this.OnJump(ctx: InputAction.CallbackContext) =
        // print "Move.fs :: OnJump :: function is called :)"

        if ctx.started then
            // print "Move.fs :: OnJump :: ctx.started"
            isHoldingJump <- true
            this.ApplyJump()
        else if ctx.canceled then
            // print "Move.fs :: OnJump :: ctx.canceled"
            isHoldingJump <- false

        ()

    member this.ApplyJump() =
        print "Move.fs :: ApplyJump :: In function"
        let jumpVelocity = Vector3.up * jumpForce

        sprintf "Move.fs :: ApplyJump :: velocity : %f %f" jumpVelocity.x jumpVelocity.y
        |> print

        // devrait checker si on peut sauter...

        rg.linearVelocity <- rg.linearVelocity + jumpVelocity

    member _.Update() =
        let movement = moveAction.ReadValue<Vector2>()

        rg.linearVelocity <- Vector3(movement.x * movementSpeed, rg.linearVelocity.y, 0f)

        ()
