using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private const string playerNamePrefKey = "PlayerName";

    private void OnValidate()
    {
        if (!inputField)
        {
            inputField = GetComponent<TMP_InputField>();
        }
    }

    private void Start()
    {
        string playerName = PlayerPrefs.GetString(playerNamePrefKey, "Player");
        inputField.text = playerName;
        PhotonNetwork.NickName = playerName;
    }

    public void SetPlayerName(string value)
    {
        Debug.LogFormat("SetPlayerName: {0}", value);

        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
