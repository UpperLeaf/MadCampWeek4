﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject gun;

    [SerializeField]
    private GameObject hammer;

    private Animator animator;

    [SerializeField]
    private Animator hammerAnimator;

    private Vector3 walkArmPos = new Vector3(0.004f, -0.01f, -2);
    private Vector3 idleArmPos = new Vector3(0.032f, -0.061f, -2);

    private Vector3 walkGunPos = new Vector3(-0.28f, 0.2f, -1f);
    private Vector3 idleGunPos = new Vector3(-0.28f, 0f, -1f);


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected == false)
            return;

        Move();
    }
    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("Walk"))
        {
            transform.localPosition = walkArmPos;
            gun.transform.localPosition = walkGunPos;
        }
        else
        {
            transform.localPosition = idleArmPos;
            gun.transform.localPosition = idleGunPos;
        }
        animator.SetFloat("Walk", h * h + v * v);
    }

    public void DigStart()
    {
        gun.SetActive(false);
        hammer.SetActive(true);
        hammerAnimator.SetTrigger("Dig");
    }

    public void DigEnd()
    {
        gun.SetActive(true);
        hammer.SetActive(false);
    }
}