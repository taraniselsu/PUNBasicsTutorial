using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimatorManager : MonoBehaviourPun, InputActions.IMovementActions
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float turnSpeed = 180f; // degrees/second
    [SerializeField] private float jumpHeight = 1;
    [SerializeField] private float gravityMultiplier = 1;
    [SerializeField] private float forwardSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    private InputActions inputActions;
    private float forwardInput = 0f;
    private float turnInput = 0f;
    private bool jumpInput = false;

    private float currentForward = 0f;
    private float currentTurn = 0f;

    private float forwardChangeVelocity = 0f;
    private float turnChangeVelocity = 0f;

    private float verticalVelocity = 0f;

    private void OnValidate()
    {
        if (!characterController)
        {
            characterController = GetComponent<CharacterController>();
        }
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            if (inputActions == null)
            {
                inputActions = new InputActions();
                inputActions.Movement.SetCallbacks(this);
            }

            inputActions.Movement.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Movement.Disable();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        forwardInput = Mathf.Clamp01(temp.y);
        turnInput = temp.x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
        {
            jumpInput = true;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        Move();
        UpdateAnimator();
    }

    private void Move()
    {
        float gravity = Physics.gravity.y * gravityMultiplier;

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = 0f;
            }

            if (jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -3f * gravity);
                jumpInput = false;
            }
        }
        verticalVelocity += gravity * Time.fixedDeltaTime;

        currentForward = Mathf.SmoothDamp(currentForward, forwardInput, ref forwardChangeVelocity, forwardSmoothTime);
        Vector3 forwardDir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 playerVelocity = forwardDir * (currentForward * moveSpeed);
        playerVelocity.y = verticalVelocity;

        characterController.Move(playerVelocity * Time.fixedDeltaTime);

        currentTurn = Mathf.SmoothDamp(currentTurn, turnInput, ref turnChangeVelocity, turnSmoothTime);
        float turnVelocity = currentTurn * turnSpeed;

        transform.Rotate(Vector3.up, turnVelocity * Time.fixedDeltaTime);
    }

    private void UpdateAnimator()
    {
        float forward = currentForward;
        float turn = currentTurn;
        bool grounded = characterController.isGrounded;
        float jump = verticalVelocity;

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Turn", turn);
        animator.SetBool("Crouch", false);
        animator.SetBool("OnGround", grounded);
        animator.SetFloat("Jump", jump);
        animator.SetFloat("JumpLeg", 0);
    }
}
