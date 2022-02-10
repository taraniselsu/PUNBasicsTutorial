using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimatorManager : MonoBehaviour, InputActions.IMovementActions
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float directionDampTime = 0.25f;

    private InputActions inputActions;
    private Vector3 moveInput = Vector3.zero;
    private bool jump = false;

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
        if (inputActions == null)
        {
            inputActions = new InputActions();
            inputActions.Movement.SetCallbacks(this);
        }
        inputActions.Movement.Enable();
    }

    private void OnDisable()
    {
        inputActions.Movement.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        Debug.LogFormat("OnMove: {0}", temp);
        moveInput = new Vector3(temp.x, 0, Mathf.Clamp01(temp.y));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.ReadValue<float>() > 0.5f;
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            if (jump)
            {
                animator.SetTrigger("Jump");
                jump = false;
            }
        }

        animator.SetFloat("Speed", moveInput.z);
        animator.SetFloat("Direction", moveInput.x, directionDampTime, Time.deltaTime);
    }
}
