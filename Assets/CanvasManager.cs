using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject stopGameMenu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stopGameMenu.SetActive(!stopGameMenu.activeInHierarchy);
        }
    }


    public void LeaveRoom()
    {
        Destroy(this.gameObject);
        PhotonNetwork.LeaveRoom();
    }

    public void ResumeGame()
    {
        stopGameMenu.SetActive(false);
    }
}
