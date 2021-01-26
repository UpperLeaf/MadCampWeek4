using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The Prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject losePanel;

    [SerializeField]
    private GameObject map;

    public static List<PlayerManager> playerManagers = new List<PlayerManager>();

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region MonoBehaviour Callbacks
    
    private void Start()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Array.Sort(players, (player1, player2) => (player1.ActorNumber > player2.ActorNumber) ? -1 : 1);

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsVisible = false;

        for (int i = 0; i < players.Length; i++)
        {
            if(players[i].Equals(PhotonNetwork.LocalPlayer))
                PhotonNetwork.Instantiate(PlayerManager.LocalPlayerInstance.name, GameObject.Find("Spawn" + i).transform.position, Quaternion.identity, 0);
        }
    }

    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
    
    public void GameWin()
    {
        winPanel.SetActive(true);
    }

    public void GameLose()
    {
        losePanel.SetActive(true);   
    }

    public void CheckGameOver()
    {
        if(playerManagers.Count <= 1)
        {
            GameWin();
        }
    }
}
