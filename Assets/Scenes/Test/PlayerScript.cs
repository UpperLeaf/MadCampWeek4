﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    private float speed = 3f;

    private bool isDead = false;

    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime * transform.right);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime * transform.right);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(speed * Time.deltaTime * transform.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-speed * Time.deltaTime * transform.up);
        }
    }
}
