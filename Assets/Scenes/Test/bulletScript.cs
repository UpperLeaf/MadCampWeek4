﻿using Photon.Pun;
using UnityEngine;

public class bulletScript : MonoBehaviourPunCallbacks
{
    public GameObject player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) return;
        
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IPlayer>().Damaged(0.1f);

        }

        PhotonNetwork.Destroy(this.gameObject);

    }
}