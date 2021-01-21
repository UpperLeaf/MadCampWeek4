using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviourPunCallbacks
{

    public GameObject _bullet;
    private float bulletspeed = 12f;

    private float speed = 3f;

    private bool fireable = true;

    private bool isDead = false;

    public float AttackCooltime = 5f;

    
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        
    }

    IEnumerator Reload()
    {
        Debug.Log("reloading");
        yield return new WaitForSeconds(AttackCooltime);
        fireable = true;
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed*Time.deltaTime*transform.right);
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

            Debug.Log("fire");
            fireable = false;
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            GameObject bullet = PhotonNetwork.Instantiate("bullet", transform.position, Quaternion.identity, 0);

            Vector2 bulletvelocity = (mouse - playerposition).normalized * bulletspeed;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletvelocity;

            StartCoroutine("Reload");
        }

    }
}
