using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
public class PlayerAnimatorManager : MonoBehaviourPun
{
	private Animator animator;

	private Vector3 dirc;

	#region MonoBehaviour CallBacks
	void Start()
	{
		animator = GetComponent<Animator>();
		dirc = new Vector3(1, 1, 1);
	}

    private void Update()
    {
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			return;
		if (!animator)
			return;
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
	
		if (!stateInfo.IsName("Dash"))
			Move();
	}
    #endregion

    private void Move()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		if (direction.x < 0)
			dirc.x = 1;
		else if (direction.x > 0)
			dirc.x = -1;
		transform.localScale = dirc;
		animator.SetFloat("Walk", h * h + v * v);
	}
}