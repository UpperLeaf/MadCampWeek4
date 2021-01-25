using Photon.Pun;
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
        PhotonNetwork.Instantiate(PlayerManager.LocalPlayerInstance.name, new Vector3(-17f, 7f, 0f), Quaternion.identity, 0);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        StartCoroutine("CheckGameWin");
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
