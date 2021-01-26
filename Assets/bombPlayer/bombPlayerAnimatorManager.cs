using Photon.Pun;
using UnityEngine;
public class BombPlayerAnimatorManager : AbstarctPlayerAnimatorManager
{
	[SerializeField]
	private Animator armAnimator;

	[SerializeField]
	private Animator weaponAnimator;

    protected override void Start()
	{
		base.Start();

	}
	protected override void Update()
	{
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			return;
		base.Update();
	}

	public override void DigEnd()
	{
		//Ignored
	}
	public override void DigStart()
	{
		armAnimator.SetTrigger("Dig");
        weaponAnimator.SetTrigger("Dig");
	}
}
