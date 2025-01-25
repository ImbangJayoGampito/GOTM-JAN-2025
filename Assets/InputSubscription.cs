using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSubscription : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool Interact { get; private set; } = false;
    public bool Crouch { get; private set; } = false;
    public bool Jump { get; private set; } = false;
    public bool Previous { get; private set; } = false;
    public bool Next { get; private set; } = false;
    public bool Attack { get; private set; } = false;
    public bool Sprint { get; private set; } = false;
    public bool Dragging { get; private set; } = false;
    public float Zoom { get; private set; } = 0.0f;

    private InputSystem_Actions inputActions;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();

        // Movement Input
        inputActions.Player.Move.Enable();
        inputActions.Player.Move.performed += SetMovement;
        inputActions.Player.Move.canceled += SetMovement;

        // Look Input
        inputActions.Player.Look.Enable();
        inputActions.Player.Look.performed += SetLook;
        inputActions.Player.Look.canceled += SetLook;

        // Sprint Input
        inputActions.Player.Sprint.Enable();
        inputActions.Player.Sprint.performed += SetSprint;
        inputActions.Player.Sprint.canceled += SetSprint;

        // Crouch Input
        inputActions.Player.Crouch.Enable();
        inputActions.Player.Crouch.performed += SetCrouch;
        inputActions.Player.Crouch.canceled += SetCrouch;

        // Drag Input
        inputActions.Player.DragScreen.Enable();
        inputActions.Player.DragScreen.performed += SetDrag;
        inputActions.Player.DragScreen.canceled += SetDrag;

        inputActions.Player.Zoom.Enable();
        inputActions.Player.Zoom.performed += SetZoom;
        inputActions.Player.Zoom.canceled += SetZoom;
    }

    void OnDisable()
    {
        // Disable all input actions
        inputActions.Player.Move.performed -= SetMovement;
        inputActions.Player.Move.canceled -= SetMovement;
        inputActions.Player.Move.Disable();

        inputActions.Player.Look.performed -= SetLook;
        inputActions.Player.Look.canceled -= SetLook;
        inputActions.Player.Look.Disable();

        inputActions.Player.Sprint.performed -= SetSprint;
        inputActions.Player.Sprint.canceled -= SetSprint;
        inputActions.Player.Sprint.Disable();

        inputActions.Player.Crouch.performed -= SetCrouch;
        inputActions.Player.Crouch.canceled -= SetCrouch;
        inputActions.Player.Crouch.Disable();

        inputActions.Player.DragScreen.performed -= SetDrag;
        inputActions.Player.DragScreen.canceled -= SetDrag;
        inputActions.Player.DragScreen.Disable();


        inputActions.Player.Zoom.performed -= SetZoom;
        inputActions.Player.Zoom.canceled -= SetZoom;
        inputActions.Player.Zoom.Disable();
    }

    void Update()
    {
        Attack = inputActions.Player.Attack.WasPressedThisFrame();
        Interact = inputActions.Player.Interact.WasPressedThisFrame();
        Sprint = inputActions.Player.Sprint.IsPressed(); // Check if Sprint is currently pressed
    }

    void SetMovement(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }

    void SetSprint(InputAction.CallbackContext ctx)
    {
        Sprint = ctx.performed; // Set Sprint to true when the action is performed
    }

    void SetCrouch(InputAction.CallbackContext ctx)
    {
        Crouch = ctx.performed; // Use ctx.performed for crouch
    }

    void SetDrag(InputAction.CallbackContext ctx)
    {
        Dragging = ctx.performed; // Use ctx.performed for dragging
    }
    void SetZoom(InputAction.CallbackContext ctx)
    {
        Zoom = ctx.ReadValue<float>();
    }
}