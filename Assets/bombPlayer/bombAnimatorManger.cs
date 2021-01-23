using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
	public class bombAnimatorManger : MonoBehaviourPun
	{
		#region Private Fields

		[SerializeField]
		private float directionDampTime = 0.25f;
		Animator animator;

		private Vector3 dirc;
		#endregion

		#region MonoBehaviour CallBacks
		void Start()
		{
			animator = GetComponent<Animator>();
			dirc = new Vector3(1, 1, 1);
		}
		void Update()
		{
			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
				return;

			if (!animator)
				return;

			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			Move();

			if (Input.GetMouseButtonUp(0))
			{
				Attack();
			}
		}

		#endregion


		private void Move()
		{
			float h = Input.GetAxisRaw("Horizontal");
			float v = Input.GetAxisRaw("Vertical");


            //if (h < 0)
            //    dirc.x = 1;
            //else if (h > 0)
            //    dirc.x = -1;

            //transform.localScale = dirc;

            animator.SetFloat("walk", h * h + v * v);
		}

		private void Attack()
		{
			animator.SetTrigger("Attack");
		}
	}
}
