using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombPlayerScript : AbstractPlayerScript
{
    [SerializeField]
    private GameObject _bomb;

    [SerializeField]
    private GameObject _BigBomb;
    
    [SerializeField]
    private bool skillAvailable = true;

    [SerializeField]
    private float attackDelayTime = 0.5f;

    [SerializeField]
    private float reloadTime = 2f;

    [SerializeField]
    private float skillCoolTime= 1f;

    [SerializeField]
    private int max_bomb = 5;

    [SerializeField]
    private int current_bomb;
    
    private bool isAttackable;

    private bool reloading = false;

    private Animator weaponAnimator;
    private Animator armAnimator;

    [SerializeField]
    private Sprite attackImage;

    [SerializeField]
    private Sprite skillImage;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isAttackable = true;
        current_bomb = max_bomb;
               
        Animator[] animators = GetComponentsInChildren<Animator>();
        
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.name == "arm")
            {
                armAnimator = animator;
            }
            else if (animator.gameObject.name == "bomb")
            {
                weaponAnimator = animator;
            }
        }

    }

    public override void SetSkillUiController(SkillUIController uIController)
    {

        uIController.SetImage(SkillUIController.SkillType.Attack, attackImage);
        uIController.SetImage(SkillUIController.SkillType.Skill, skillImage);
        uIController.bombInit();

        base.SetSkillUiController(uIController);
    }

    protected override void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;
        base.Update();
        if (!reloading && current_bomb < max_bomb)
        {
            reloading = true;
            StartCoroutine("Reload");
        }
        Skill();
    }
    protected override void Attack()
    {
        if (Input.GetMouseButtonUp(0) && current_bomb > 0 && isAttackable)
        {
            if (skillUIController != null)
                skillUIController.UseSkill(SkillUIController.SkillType.Attack, attackDelayTime);    
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            photonView.RPC("ThrowBomb", PhotonTargets.All, new object[] { mouse, playerposition });
            StartCoroutine("AttackDelay");
        }
    }

    [PunRPC]
    public void ThrowBomb(Vector2 mouse, Vector2 playerposition)
    {
        AttackTrigger();
        isAttackable = false;
        current_bomb--;
        UpdateAmmo(SkillUIController.SkillType.Attack, current_bomb);
        _bomb.GetComponent<bombObject>().Target = mouse;
        _bomb.GetComponent<bombObject>().startposition = playerposition;
        Instantiate(_bomb);
    }

    [PunRPC]
    public void ThrowBigBomb(Vector2 mouse, Vector2 playerposition)
    {
        AttackTrigger();
        _BigBomb.GetComponent<bigBombScript>().Target = mouse;
        _BigBomb.GetComponent<bigBombScript>().startposition = playerposition;
        Instantiate(_BigBomb);
    }

    private void Skill()
    {
        if (Input.GetKeyUp(KeyCode.Space) && skillAvailable)
        {
            skillAvailable = false;
            if (skillUIController != null)
                skillUIController.UseSkill(SkillUIController.SkillType.Skill, skillCoolTime);
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            photonView.RPC("ThrowBigBomb", PhotonTargets.All, new object[] { mouse, playerposition });
            StartCoroutine("SkillCool");
        }
    }

    private void AttackTrigger()
    {
        animator.SetTrigger("Attack");
        weaponAnimator.SetTrigger("Attack");
        armAnimator.SetTrigger("Attack");
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        current_bomb++;
        UpdateAmmo(SkillUIController.SkillType.Attack, current_bomb);
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelayTime);
        isAttackable = true;
    }

    IEnumerator SkillCool()
    {
        yield return new WaitForSeconds(skillCoolTime);
        skillAvailable = true;
    }
}
