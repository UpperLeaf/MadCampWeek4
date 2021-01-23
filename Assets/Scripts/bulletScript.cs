using Photon.Pun;
using System.Collections;
using UnityEngine;

public class bulletScript : MonoBehaviourPunCallbacks
{
    public GameObject _player;

    [SerializeField]
    private float deleteTime;

    private void Start()
    {
        deleteTime = 1f;
        StartCoroutine("Delete");
    }

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
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(deleteTime);
        Destroy(gameObject);
    }
}
