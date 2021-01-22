using Photon.Pun;
using UnityEngine;

public class bulletScript : MonoBehaviourPunCallbacks
{
    public GameObject _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_player == collision.gameObject)
            return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IPlayer>().Damaged(0.1f);

        }
        Destroy(gameObject);
    }
}
