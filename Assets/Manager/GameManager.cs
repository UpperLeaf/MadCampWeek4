using Photon.Pun;
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
    

    public static List<PlayerManager> playerManagers = new List<PlayerManager>();

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
        PlayerManagerGameStart();
    }

    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private Methods
    private void PlayerManagerGameStart()
    {
        foreach (PhotonView view in PhotonNetwork.PhotonViewCollection)
        {
            PlayerManager playerManager = view.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.GameStart();
                playerManagers.Add(playerManager);
            }

            if (!view.IsMine)
            {
                Destroy(view.gameObject.GetComponent<Light2D>());
                foreach (Transform transform in view.gameObject.transform)
                {
                    if (transform.name == "sight")
                    {
                        Destroy(transform.gameObject);
                        break;
                    }
                }
            }
        }
    }
    #endregion
    IEnumerator CheckGameWin()
    {
        bool isWin = false;
        while (!isWin)
        {
            isWin = true;
            foreach(PlayerManager playerManager in playerManagers)
            {
                if(!playerManager.photonView.IsMine && !playerManager.IsDied())
                {
                    isWin = false;
                }
            }
            yield return new WaitForSeconds(1);
        }
        GameWin();
    }

    public void GameWin()
    {
        //GameObject winMenu = GameObject.Find("WinnerMenu");
        //winMenu.SetActive(true);
    }
    
}
