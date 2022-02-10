using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviourPunCallbacks, InputActions.IFiringActions
{
    [SerializeField] private GameObject beams;
    private GameManager gameManager;

    private InputActions inputActions;
    private bool isFiring = false;
    private float health = 1f;

    public override void OnEnable()
    {
        base.OnEnable();
        if (inputActions == null)
        {
            inputActions = new InputActions();
            inputActions.Firing.SetCallbacks(this);
        }
        inputActions.Firing.Enable();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        beams.SetActive(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        inputActions.Firing.Disable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        isFiring = context.ReadValue<float>() > 0.5f;
    }

    private void Update()
    {
        if (isFiring != beams.activeInHierarchy)
        {
            beams.SetActive(isFiring);
        }

        if (photonView.IsMine && health <= 0)
        {
            gameManager.LeaveRoom();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("OnTriggerEnter: {0}", other.name);

        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }

        health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.LogFormat("OnTriggerEnter: {0}", other.name);

        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }

        health -= 0.1f * Time.deltaTime;
    }
}
