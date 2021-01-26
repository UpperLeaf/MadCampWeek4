using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombPlayerScript : AbstractPlayerScript
{
    public GameObject _player;

    public GameObject _bomb;

    public GameObject _BigBomb;

    [SerializeField]
    private bool fireable = true;
    [SerializeField]
    private bool reloading = false;
    [SerializeField]
    private bool skillAvailable = true;

    public float AttackDelayTime = 0.5f;
    public float ReloadTime = 2f;
    public float SkillCoolTime = 1f;
    public int max_bomb = 5;

    [SerializeField]
    private int current_bomb;

    private Animator weaponAnimator;
    private Animator armAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _player = gameObject;
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

    IEnumerator Reload()
    {
        Debug.Log("reloading");
        yield return new WaitForSeconds(ReloadTime);
        reloading = false;
        current_bomb++;
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(AttackDelayTime);
        fireable = true;
    }

    IEnumerator SkillCool()
    {
        Debug.Log("SkillCool");
        yield return new WaitForSeconds(SkillCoolTime);
        skillAvailable = true;
        
    }

    protected override void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        base.Update();

        if (Input.GetMouseButtonUp(0) && current_bomb > 0 && fireable)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            photonView.RPC("ThrowBomb", PhotonTargets.All, new object[] { mouse, playerposition });
        }

        if (!reloading && current_bomb < max_bomb)
        {
            reloading = true;
            StartCoroutine("Reload");
        }

        if (Input.GetKeyUp(KeyCode.Space) && skillAvailable)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            photonView.RPC("ThrowBigBomb", PhotonTargets.All, new object[] { mouse, playerposition });
        }
    }
    
    //private void Attack()
    //{
    //    animator.SetTrigger("Attack");
    //    weaponAnimator.SetTrigger("Attack");
    //    armAnimator.SetTrigger("Attack");
    //}

    [PunRPC]
    public void ThrowBomb(Vector2 mouse, Vector2 playerposition)
    {
        Attack();
        fireable = false;
        current_bomb--;
        _bomb.GetComponent<bombObject>().Target = mouse;
        _bomb.GetComponent<bombObject>().startposition = playerposition;
        GameObject bomb = Instantiate(_bomb);
        
        StartCoroutine("AttackDelay");
    }

    [PunRPC]
    public void ThrowBigBomb(Vector2 mouse, Vector2 playerposition)
    {
        Attack();
        skillAvailable = false;
        _BigBomb.GetComponent<bigBombScript>().Target = mouse;
        _BigBomb.GetComponent<bigBombScript>().startposition = playerposition;
        
        GameObject bomb = Instantiate(_BigBomb);

        StartCoroutine("SkillCool");
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
