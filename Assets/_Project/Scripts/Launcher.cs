using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressLabel;
    [SerializeField] private byte maxPlayersPerRoom = 4;

    private bool isConnecting = false;

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
            JoinRandomOrCreateRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void JoinRandomOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = maxPlayersPerRoom };
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        if (isConnecting)
        {
            JoinRandomOrCreateRoom();
            isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected: {0}", cause);
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
        isConnecting = false;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("OnJoinedRoom: {0}", PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("OnJoinedRoom: loading level 'Room for 1'");
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("OnCreateRandomFailed: {0} [{1}]", message, returnCode);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("OnJoinRoomFailed: {0} [{1}]", message, returnCode);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat("OnJoinRandomFailed: {0} [{1}]", message, returnCode);
    }
}
