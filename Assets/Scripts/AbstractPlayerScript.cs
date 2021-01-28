using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public abstract class AbstractPlayerScript : MonoBehaviourPunCallbacks
{
    protected CinemachineBasicMultiChannelPerlin perlin;

    protected SkillUIController skillUIController;

    protected Animator animator;
    
    protected Vector2 velocity;
    
    protected float horizontal;

    protected float vertical;

    protected AudioSource audioSource;

    [SerializeField]
    private float speed = 150f;
    
    public void SetCinemachineBasicMultiChannelPerlin(CinemachineBasicMultiChannelPerlin perlin)
    {
        this.perlin = perlin;
    }

    public void ShakeCameraAttack(float intentsity, float time)
    {
        if (photonView.IsMine)
        {
            perlin.m_AmplitudeGain = intentsity;
            StartCoroutine("ShakeTime", time);
        }
    }

    IEnumerator ShakeTime(float time)
    {
        yield return new WaitForSeconds(time);
        perlin.m_AmplitudeGain = 0;
    }

    public virtual void SetSkillUiController(SkillUIController uIController)
    {
        skillUIController = uIController;
    }

    public void UpdateAmmo(SkillUIController.SkillType type, int ammo)
    {
        if(skillUIController != null)
            skillUIController.updateAmmo(type, ammo);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        Move();
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Dig"))
        {
            Attack();
        }
    }

    protected abstract void Attack();
    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        velocity.x = horizontal;
        velocity.y = vertical;
        
        velocity = velocity.normalized;

        velocity.x *= speed;
        velocity.y *= speed;

        transform.Translate(velocity * Time.deltaTime);
    }
}

