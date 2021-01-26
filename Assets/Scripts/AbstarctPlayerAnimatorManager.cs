using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstarctPlayerAnimatorManager : MonoBehaviourPunCallbacks
{
    protected Animator animator;

    private Vector3 playerScaleDirc;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        playerScaleDirc = Vector3.one;
    }

    protected virtual void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (direction.x < 0)
            playerScaleDirc.x = 1;
        else if (direction.x > 0)
            playerScaleDirc.x = -1;
        transform.localScale = playerScaleDirc;
        animator.SetFloat("Walk", h * h + v * v);
    }
    public abstract void DigStart();
    public abstract void DigEnd();
}
