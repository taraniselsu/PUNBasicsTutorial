using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        Assert.IsNotNull(playerPrefab);

        if (!PlayerManager.HasLocalPlayerInstance)
        {
            Debug.LogFormat("Instantiating local player in {0}", SceneManagerHelper.ActiveSceneName);
            Vector3 temp = Random.insideUnitCircle;
            Vector3 position = new Vector3(temp.x, 5f, temp.y);
            PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Local player already instantiated for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("OnPlayerEnteredRoom: {0}", newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("OnPlayerEnteredRoom: IsMasterClient");
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom: {0}", otherPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("OnPlayerLeftRoom: IsMasterClient");
            LoadArena();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Trying to load a level but we are not the master client");
        }

        string levelName = "Room for " + PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.LogFormat("Loading level: {0}", levelName);
        PhotonNetwork.LoadLevel(levelName);
    }
}
