using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombPlayerScript : MonoBehaviourPunCallbacks
{
    public GameObject _player;

    public GameObject _bomb;

    private float bulletspeed = 12f;

    private float speed = 3f;

    private bool fireable = true;

    private bool isDead = false;

    public float AttackCooltime = 0.3f;

    private GameObject bulletInstance;

    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        _player = gameObject;
    }

    IEnumerator Reload()
    {
        Debug.Log("reloading");
        yield return new WaitForSeconds(AttackCooltime);
        fireable = true;
        Debug.Log(fireable);

    }

    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime * transform.right);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime * transform.right);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(speed * Time.deltaTime * transform.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-speed * Time.deltaTime * transform.up);
        }

        if (Input.GetMouseButtonUp(0) && fireable)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;

            Debug.Log("mouseposition  " + mouse);
            Debug.Log("playerposition  " + playerposition);

            ThrowBomb(mouse, playerposition);

            //photonView.RPC("ThrowBomb", PhotonTargets.All, new object[] { mouse, playerposition });
        }
    }


    [PunRPC]
    public void ThrowBomb(Vector2 mouse, Vector2 playerposition)
    {
        fireable = false;

        _bomb.GetComponent<bombObject>().Target = mouse;
        _bomb.GetComponent<bombObject>().startposition = playerposition;
        GameObject bomb = Instantiate(_bomb);
        

        StartCoroutine("Reload");
    }
}
