using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{

    #region
    const string playerNamePrefkey = "Playername";
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = GetComponent<InputField>();
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

    #region
    public void SetPlayername(string value)
    {
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
