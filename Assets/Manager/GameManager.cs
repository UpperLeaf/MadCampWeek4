using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The Prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnterRoom() {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnterRoom Is Master Client {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom Is Master Client {0}", PhotonNetwork.IsMasterClient);
            //LoadArena();
        }
    }
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        if (playerPrefab == null)
            Debug.LogError("Missing Prefab");
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("Instantiate LocalPlayer");
                GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                _player.GetComponent<PlayerScript>().id = PhotonNetwork.CountOfPlayers;
            }
        }
    }
    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion


    #region Private Methods
    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("PhotonNetwork : Trying to Load a Level But we are not the Master Client");
        }
        Debug.LogFormat("Photon Network : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Game");
    }
    #endregion

}
