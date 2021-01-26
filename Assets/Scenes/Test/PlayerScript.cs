using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : AbstractPlayerScript
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Animator fireAnimator;

    private float dashSpeed = 700f;

    private float bulletSpeed = 12f;

    private bool isDashAble;

    private bool isAttackAble;

    [SerializeField]
    private float dashCoolTime;

    [SerializeField]
    private float attackCoolTime;
    
    private SkillUI dashUI;

    protected override void Start()
    {
        base.Start();
        isAttackAble = true;
        isDashAble = true;
        dashCoolTime = 3f;
        attackCoolTime = 0.2f;
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
            velocity.x = dashSpeed * horizontal * Time.deltaTime;
            velocity.y = dashSpeed * vertical * Time.deltaTime;
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

    private void ShakeCameraAttack(float intentsity, float time)
    {
        perlin.m_AmplitudeGain = intentsity;
        StartCoroutine("ShakeTime", time);
    }

    IEnumerator ShakeTime(float time)
    {
        yield return new WaitForSeconds(time);
        perlin.m_AmplitudeGain = 0;
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
