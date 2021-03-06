﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : AbstractPlayerScript
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Animator fireAnimator;

    [SerializeField]
    private float dashSpeed = 10f;

    private float bulletSpeed = 12f;

    private bool isDashAble;

    private bool isAttackAble;

    [SerializeField]
    private float dashCoolTime;

    [SerializeField]
    private float attackCoolTime;
    
    private SkillUI dashUI;

    [SerializeField]
    private Sprite attackImage;

    [SerializeField]
    private Sprite skillImage;

    [SerializeField]
    private AudioClip bulletAttackAudioClip;

    protected override void Start()
    {
        base.Start();
        isAttackAble = true;
        isDashAble = true;
        dashCoolTime = 3f;
        attackCoolTime = 0.2f;

    }

    public override void SetSkillUiController(SkillUIController uIController)
    {
        uIController.DisableAmmoText(SkillUIController.SkillType.Attack);
        uIController.SetImage(SkillUIController.SkillType.Attack, attackImage);
        uIController.SetImage(SkillUIController.SkillType.Skill, skillImage);
        uIController.rifleInit();
        
        base.SetSkillUiController(uIController);
    }


    protected override void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Dash"))
        {
            base.Update();
            Dash();
        }
        else
        {
            velocity.x = horizontal;
            velocity.y = vertical;
            
            velocity.Normalize();

            velocity.x *= dashSpeed;
            velocity.y *= dashSpeed;

            transform.Translate(velocity * Time.deltaTime);
        }
    }
    protected override void Attack()
    {
        if (Input.GetMouseButtonUp(0) && isAttackAble)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            Vector2 bulletvelocity = (mouse - playerposition).normalized * bulletSpeed;

            ShakeCameraAttack(1f, 0.1f);
            
            audioSource.PlayOneShot(bulletAttackAudioClip);

            if (skillUIController != null)
                skillUIController.UseSkill(SkillUIController.SkillType.Attack, attackCoolTime);

            photonView.RPC("FireBullet", PhotonTargets.All, new object[] { bulletvelocity });
        }
    }

    [PunRPC]
    public void FireBullet(Vector2 bulletvelocity)
    {
        isAttackAble = false;

        fireAnimator.SetTrigger("Attack");
        
        bullet.transform.position = transform.position;
        bullet.GetComponent<bulletScript>()._player = gameObject;
        
        GameObject _bullet = Instantiate(bullet);
        _bullet.GetComponent<Rigidbody2D>().velocity = bulletvelocity;
        StartCoroutine("Reload");
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDashAble)
        {
            isDashAble = false;

            if (skillUIController != null)
                skillUIController.UseSkill(SkillUIController.SkillType.Skill, dashCoolTime);
            
            photonView.RPC("SetDashTrigger", PhotonTargets.All);
            StartCoroutine("DashEnd");
            StartCoroutine("DashCoolTime");
        }
    }

    public void SetDashUI(SkillUI skill)
    {
        dashUI = skill;
    }
    
    [PunRPC]
    private void SetDashTrigger()
    {
        animator.SetTrigger("Dash");
    }

    IEnumerator DashEnd()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("DashEnd");
    }

    IEnumerator DashCoolTime()
    {
        yield return new WaitForSeconds(dashCoolTime);
        isDashAble = true;
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(attackCoolTime);
        isAttackAble = true;
    }
}
