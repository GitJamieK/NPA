using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 15f;
    public float rotationSpeed = 50f;

    private PlayerControls playercontrols;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRightMouseHeld;
    
    private Rigidbody rb;

    private void Awake()
    {
        playercontrols = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playercontrols.Enable();
        playercontrols.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playercontrols.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        
        playercontrols.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playercontrols.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        
        playercontrols.Player.RightClick.started += ctx => isRightMouseHeld = true;
        playercontrols.Player.RightClick.canceled += ctx => isRightMouseHeld = false;
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        MovePlayer();
        if (isRightMouseHeld)
        {
            RotatePlayer();
        }
    }
    
    private void MovePlayer()
    {
        if (!IsOwner) return;
        
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.Self);
    }
    
    private void RotatePlayer()
    {
        if (lookInput.sqrMagnitude < 0.1f) return;

        float yaw = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);
    }
}