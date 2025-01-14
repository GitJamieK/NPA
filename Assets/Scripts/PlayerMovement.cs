using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private PlayerControls playercontrols;
    private Vector2 moveInput;

    private void Awake()
    {
        playercontrols = new PlayerControls();
    }

    private void OnEnable()
    {
        playercontrols.Enable();
        playercontrols.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playercontrols.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}