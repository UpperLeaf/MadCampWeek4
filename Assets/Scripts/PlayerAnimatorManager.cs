using Photon.Pun;
using System.Collections;
using UnityEngine;
public class PlayerAnimatorManager : MonoBehaviourPun
{
	#region Public Fields

	#endregion

	#region Private Fields
	private GameObject _player;

	[SerializeField]
	private Animator fireAnimator;

    [SerializeField]
	private float directionDampTime = 0.25f;

	[SerializeField]
	private float dashSpeed;

	private Animator animator;

	private Vector3 dirc;

	private bool fireable = true;

	public float AttackCooltime = 0.3f;

	public GameObject _bullet;

	private float bulletspeed = 12f;
	#endregion

	#region MonoBehaviour CallBacks
	void Start()
	{
		animator = GetComponent<Animator>();
		_player = gameObject;
		dirc = new Vector3(1, 1, 1);
	}

    private void FixedUpdate()
    {
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			return;
		if (!animator)
			return;
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (!stateInfo.IsName("Dash"))
		{
			Move();
			Attack();
			
		}
	}
    #endregion

    private void Move()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		if (h < 0)
			dirc.x = 1;
		else if (h > 0)
			dirc.x = -1;
		transform.localScale = dirc;
		animator.SetFloat("Walk", h * h + v * v);
	}

	private void Attack()
	{
		if (Input.GetMouseButtonUp(0) && fireable)
		{
			Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 playerposition = transform.position;
			Vector2 bulletvelocity = (mouse - playerposition).normalized * bulletspeed;
			photonView.RPC("FireBullet", PhotonTargets.All, new object[] { bulletvelocity });
		}
	}

	[PunRPC]
	public void FireBullet(Vector2 bulletvelocity)
	{
		fireAnimator.SetTrigger("Attack");
		fireable = false;
		_bullet.transform.position = transform.position;
		_bullet.GetComponent<bulletScript>()._player = _player;
		GameObject bullet = Instantiate(_bullet);
		bullet.GetComponent<Rigidbody2D>().velocity = bulletvelocity;
		StartCoroutine("Reload");
	}

	IEnumerator Reload()
	{
		Debug.Log("reloading");
		yield return new WaitForSeconds(AttackCooltime);
		fireable = true;
		Debug.Log(fireable);
	}
}