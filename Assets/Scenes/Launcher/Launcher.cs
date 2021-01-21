using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    #endregion

    [Tooltip("방에 입장할 수 있는 최대 플레이어 인원 수")]
    [SerializeField]
    private byte maxPlayersPerRoom;

    [Tooltip("Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    private GameObject progressLabel;

    #region Private Fields

    private string gameVersion = "1";
    private bool isConnecting;
    #endregion



    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            //Random룸으로 입장
            //실패시 OnJoinRandomFailed()메서드를 콜백
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //룸에 입장하기 위해서는 먼저 PhotonNetwork에 입장해야한다.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region MonoBehaviorPunCallBacks CallBacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was Called By PUN");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the Room for 1");
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN With reason {0}", cause);
    }
    #endregion
}