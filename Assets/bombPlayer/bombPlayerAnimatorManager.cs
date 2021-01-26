using Photon.Pun;
using UnityEngine;
public class BombPlayerAnimatorManager : AbstarctPlayerAnimatorManager
{
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
}
