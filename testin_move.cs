using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testin_move : MonoBehaviour
{

    // script Álvaro 
    public PlayerInputActions player_controls;
    Vector2 input_movement_data = Vector2.zero;
    CharacterController character_controller;
    bool pressed_jump;


    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private void Awake()
    {
        player_controls = new PlayerInputActions();

        player_controls.Player.Move.performed += Move_performed =>
        {
            input_movement_data = Move_performed.ReadValue<Vector2>();
        };

        player_controls.Player.Move.canceled += Move_canceled =>
        {
            input_movement_data = Move_canceled.ReadValue<Vector2>();
        };

        player_controls.Player.Jump.performed += Jump_performed =>
        {
            pressed_jump = Jump_performed.ReadValueAsButton();
        };
        

    }

    

    private void Start()
    {
        character_controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = character_controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0.0f)
        {
            playerVelocity.y = 0.0f;
        }

        Vector3 move = new Vector3(input_movement_data.x, 0.0f, input_movement_data.y);
        character_controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (pressed_jump && groundedPlayer)
        {
            playerVelocity.y += jumpHeight * gravityValue;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        character_controller.Move(playerVelocity * Time.deltaTime);
    }
    private void OnEnable()
    {
        player_controls.Enable();
    }
    private void OnDisable()
    {
        player_controls.Disable();
    }
}
