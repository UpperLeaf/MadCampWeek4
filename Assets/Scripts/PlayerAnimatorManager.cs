using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
public class PlayerAnimatorManager : AbstarctPlayerAnimatorManager
{
	[SerializeField]
	private PlayerArmManager armManager;

    #region MonoBehaviour CallBacks
    protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
    {
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			return;
		
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (!stateInfo.IsName("Dash"))
			base.Update();
	}
	public override void DigStart()
	{
		armManager.DigStart();
	}

	public override void DigEnd()
    {
		armManager.DigEnd();
    }
    #endregion
}