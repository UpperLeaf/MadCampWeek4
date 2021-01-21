using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{

    #region Private Constants
    const string playerNamePrefkey = "Playername";
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        string defaultName = string.Empty;
        TMP_InputField _inputField = GetComponent<TMP_InputField>();
        if(_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefkey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefkey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }
    #endregion

    #region Public Methods
    public void SetPlayerName(string value)
    {
        Debug.Log(value);
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefkey, value);
    }
    #endregion
}
