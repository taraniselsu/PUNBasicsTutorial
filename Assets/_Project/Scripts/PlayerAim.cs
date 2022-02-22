using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviourPun
{
    [SerializeField] private Rig aimingRig;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask layerMask;

    private void OnEnable()
    {
        aimingRig.weight = 1f;
    }

    private void OnDisable()
    {
        // Should fix MissingReferenceExceptions when quiting caused by the animation rig
        // still trying to access the transforms
        aimingRig.weight = 0f;
    }

    private void Start()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        int localPlayerLayer = LayerMask.NameToLayer("LocalPlayer");
        transform.root.gameObject.layer = localPlayerLayer;
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        Aim();
    }

    private void Aim()
    {
        Camera theCamera = Camera.main;

        Ray ray = theCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMask))
        {
            target.position = hit.point;
        }
    }
}
