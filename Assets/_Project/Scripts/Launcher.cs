using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressLabel;
    [SerializeField] private byte maxPlayersPerRoom = 4;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
    }

    public void Connect()
    {
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected: {0}", cause);
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat("OnJoinRandomFailed: {0} {1}", returnCode, message);
        PhotonNetwork.CreateRoom("room", new RoomOptions() { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("OnJoinedRoom: {0}", PhotonNetwork.CurrentRoom.Name);
    }
}
