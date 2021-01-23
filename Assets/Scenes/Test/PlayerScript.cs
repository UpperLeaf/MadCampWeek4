using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    private float speed = 300f;

    private float dashSpeed = 900f;

    private bool isDead = false;

    private Rigidbody2D body;

    private Vector3 velocity;

    private Animator animator;

    private bool isDashAble;

    [SerializeField]
    private float dashCooltime;
    
    private float horizontal;
    private float vertical;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        isDashAble = true;
        dashCooltime = 3f;
    }

    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Dash"))
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            velocity.x = speed * horizontal * Time.deltaTime;
            velocity.y = speed * vertical * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime);
            Dash();
        }
        else
        {
            velocity.x = dashSpeed * horizontal * Time.deltaTime;
            velocity.y = dashSpeed * vertical * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime);
        }
    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDashAble)
        {
            isDashAble = false;
            photonView.RPC("SetDashTrigger", PhotonTargets.All);
            StartCoroutine("DashEnd");
            StartCoroutine("DashCoolTime");
        }
    }
    
    [PunRPC]
    private void SetDashTrigger()
    {
        animator.SetTrigger("Dash");
    }
    IEnumerator DashEnd()
    {
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("DashEnd");
    }

    IEnumerator DashCoolTime()
    {
        yield return new WaitForSeconds(dashCooltime);
        isDashAble = true;
    }
}
