using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public abstract class AbstractPlayerScript : MonoBehaviourPunCallbacks
{
    protected CinemachineBasicMultiChannelPerlin perlin;

    protected SkillUIController skillUIController;

    protected Animator animator;
    
    protected Vector3 velocity;
    
    protected float horizontal;

    protected float vertical;

    [SerializeField]
    private float speed = 300f;
    
    public void SetCinemachineBasicMultiChannelPerlin(CinemachineBasicMultiChannelPerlin perlin)
    {
        this.perlin = perlin;
    }
    public void ShakeCameraAttack(float intentsity, float time)
    {
        perlin.m_AmplitudeGain = intentsity;
        StartCoroutine("ShakeTime", time);
    }

    IEnumerator ShakeTime(float time)
    {
        yield return new WaitForSeconds(time);
        perlin.m_AmplitudeGain = 0;
    }

    public void SetSkillUiController(SkillUIController uIController)
    {
        skillUIController = uIController;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
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

        velocity.x = speed * horizontal * Time.deltaTime;
        velocity.y = speed * vertical * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);
    }
}

