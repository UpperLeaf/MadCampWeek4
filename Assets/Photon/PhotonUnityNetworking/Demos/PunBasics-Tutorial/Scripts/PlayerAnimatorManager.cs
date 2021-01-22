// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerAnimatorManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player Animator Component controls.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
	public class PlayerAnimatorManager : MonoBehaviourPun 
	{
        #region Private Fields

        [SerializeField]
	    private float directionDampTime = 0.25f;
        Animator animator;

		private Vector3 dirc;
		#endregion

		#region MonoBehaviour CallBacks
	    void Start () 
	    {
	        animator = GetComponent<Animator>();
			dirc = new Vector3(1, 1, 1);
	    }
	    void Update () 
	    {
	        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true )
	            return;

	        if (!animator)
				return;
            
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			Move();
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

        }
    }
}