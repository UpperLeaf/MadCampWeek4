using Cinemachine;
using Photon.Pun;
using UnityEngine;

public abstract class AbstractPlayerScript : MonoBehaviourPunCallbacks
{
    protected CinemachineBasicMultiChannelPerlin perlin;

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

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("Init Animator");
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Move();
        Attack();
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

