using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Tooltip("Set this value to true when playing split keyboard.")]
    public bool splitKeyboardInput = false;
    [Tooltip("Ensure 1 of the 2 players' Player Number is set to 1.")]
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

    public Rigidbody heldItemQueue; // The queue holds whatever item the player is inside the PickupArea of
    public Rigidbody heldItemRB; // When the player presses the Pickup button, the Queue item gets transferred here.

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
        if (splitKeyboardInput) // horrible code that is unfortunately necessary for split keyboard controls
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
        }
        else
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
        }
        else
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
        }
        else
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
                heldItemRB = heldItemQueue; // if there's an item in the queue, transfer it to the heldItemRB.
                heldItemQueue = null; // clear the queue
                jumpHeight = 0.75f; // halve the player's jumpheight
            }
            else
            {
                heldItemRB = null; // drop the item if nothing in queue
                jumpHeight = 1.5f; // reset the player's jump height to normal
            }
        }
    }

    private void FixedUpdate()
    {
        if (heldItemRB != null)
        {
            // halve the player's jumphight if something is held
            heldItemRB.MovePosition(transform.position + Vector3.up * heldItemRB.gameObject.transform.localScale.magnitude * 0.75f); // hold the item above the player's head
            heldItemRB.MoveRotation(Quaternion.identity);
            heldItemRB.linearVelocity = Vector3.zero; // ensure it is not affected by gravity while being held
        }
    }


}