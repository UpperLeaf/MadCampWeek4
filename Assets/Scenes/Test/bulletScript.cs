using Photon.Pun;
using UnityEngine;

public class bulletScript : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
            return;
        
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IPlayer>().Damaged(0.1f);

        }
        PhotonNetwork.Destroy(gameObject);
    }
}
