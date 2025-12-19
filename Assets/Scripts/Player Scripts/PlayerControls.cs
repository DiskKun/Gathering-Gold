using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool splitKeyboardInput = false;
    public int splitKeyPlayerNumber = 1;

    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.5f;
    [SerializeField]
    private float gravityValue = -9.81f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference pickupAction;

    public Rigidbody heldItemQueue;
    public Rigidbody heldItemRB;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    Vector2 DirectionInput()
    {
        Vector2 direction = Vector2.zero;
        if (splitKeyboardInput)
        {
            if (splitKeyPlayerNumber == 1)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    direction.y = 1;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    direction.y = -1;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    direction.x = -1;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    direction.x = 1;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    direction.y = 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    direction.y = -1;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    direction.x = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    direction.x = 1;
                }
            }
            direction = direction.normalized;
        } else
        {
            direction = moveAction.action.ReadValue<Vector2>();
        }


        return direction;

    }

    bool JumpInput()
    {
        if (splitKeyboardInput)
        {
            if (splitKeyPlayerNumber == 1)
            {
                return Input.GetKeyDown(KeyCode.RightShift);
            }
            else
            {
                return Input.GetKeyDown(KeyCode.Space);
            }
        } else
        {
            return jumpAction.action.WasPressedThisFrame();
        }
        
    }

    bool PickupInput()
    {
        if (splitKeyboardInput)
        {
            if (splitKeyPlayerNumber == 1)
            {
                return Input.GetKeyDown(KeyCode.Slash);
            }
            else
            {
                return Input.GetKeyDown(KeyCode.LeftShift);
            }
        } else
        {
            return pickupAction.action.WasPressedThisFrame();
        }
        
    }


    void Update()
    {

        

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        // Read input
        Vector2 input = DirectionInput();
        
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
            transform.forward = move;

        // Jump using WasPressedThisFrame()
        if (groundedPlayer && JumpInput())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);


        // pickup
        if (PickupInput())
        {
            if (heldItemQueue)
            {
                heldItemRB = heldItemQueue;
                heldItemQueue = null;
            } else
            {
                heldItemRB = null;
            }
        }
        
        
    }

    private void FixedUpdate()
    {
        if (heldItemRB != null)
        {
            jumpHeight = 0.75f;
            heldItemRB.MovePosition(transform.position + Vector3.up * 2);
            heldItemRB.MoveRotation(Quaternion.identity);
            heldItemRB.linearVelocity = Vector3.zero;
        } else
        {
            jumpHeight = 1.5f;
        }
    }


}