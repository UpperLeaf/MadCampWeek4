using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Tilemap wall;

    public Tile hit0;
    public Tile hit1;
    public Tile hit2;

    private Animator animator;
    private Animator weaponAnimator;
    private Animator armAnimator;

    [SerializeField]
    private int num_rocks = 0;

    // Start is called before the first frame update
    void Start()
    {
            
        animator = GetComponent<Animator>();

        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.name == "arm")
            {
                armAnimator = animator;
            }
            else if (animator.gameObject.name == "bomb")
            {
                weaponAnimator = animator;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;
            Debug.Log("Dig");

            photonView.RPC("Dig", PhotonTargets.All, new object[] { mouse, playerposition });
        }
    }

    [PunRPC]
    private void DigAnim()
    {
        animator.SetTrigger("Dig");
        weaponAnimator.SetTrigger("Dig");
        armAnimator.SetTrigger("Dig");
    }

    [PunRPC]
    public void Dig(Vector2 mouse, Vector2 playerposition)
    {
        photonView.RPC("DigAnim", PhotonTargets.All);
        Vector2 dir = mouse - playerposition;

        wall = GameObject.Find("Wall").GetComponent<Tilemap>();


        float angle = Vector2.SignedAngle(Vector2.right, dir);
        Vector3Int offset = Vector3Int.zero;

        if (-45f <angle || angle <= 45f)
        {
            offset.x = 1;
        }else if(45f < angle || angle <= 135f)
        {
            offset.y = 1;
        }else if (135f < angle || angle <= -135f)
        {
            offset.x = -1;
        }else if (-135f < angle || angle <= -45f)
        {
            offset.y = -1;
        }

        Vector3Int coord = wall.WorldToCell(mouse);
        TileBase tile = wall.GetTile<Tile>(coord);
        
        if (tile == hit0)
        {
            wall.SetTile(coord, hit1);
        }
        else if (tile == hit1)
        {
            wall.SetTile(coord, hit2);
        }
        else if (tile == hit2)
        {
            wall.SetTile(coord, null);
        }
        

    }
}
