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
    private Transform wallspriterTransform;

    private AbstarctPlayerAnimatorManager animatorManager;

    public Vector3 tilemapCoordOffset;
    [SerializeField]
    private bool buildmode = false;

    
    [SerializeField]
    private int num_rocks = 0;

    private Vector3Int coord;
    private Vector3Int offset = Vector3Int.zero;
    private Tile tile;
    [SerializeField]
    private bool DigOrBuildBool;


    // Start is called before the first frame update
    void Start()
    {
        wallspriterTransform = GameObject.Find("WallSpriteRenderer").transform;
        wallspriterTransform.GetComponent<SpriteRenderer>().enabled = false;
        animator = GetComponent<Animator>();
        animatorManager = GetComponent<AbstarctPlayerAnimatorManager>();

        tilemapCoordOffset = new Vector3(1, 1, 0);

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
        if (Input.GetKeyUp(KeyCode.LeftShift) && animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;

            DigOrBuildBool = true;
            photonView.RPC("DigOrBuild", PhotonTargets.All, new object[] { mouse, playerposition });
        }


        if (Input.GetKeyUp(KeyCode.F) &&  num_rocks > 0)
        {
            if (!buildmode)
            {
                wallspriterTransform.GetComponent<SpriteRenderer>().enabled = true;
                buildmode = true;
                StartCoroutine("showExpectedWall");
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 playerposition = transform.position;
                wallspriterTransform.GetComponent<SpriteRenderer>().enabled = false;
                buildmode = false;
                DigOrBuildBool = false;
                photonView.RPC("DigOrBuild", PhotonTargets.All, new object[] { mouse, playerposition });
            }            
        }

        

        
    }


    private void DigAnim()
    {
        animator.SetTrigger("Dig");
        weaponAnimator.SetTrigger("Dig");
        armAnimator.SetTrigger("Dig");
    }

    IEnumerator showExpectedWall()
    {
        while (buildmode)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerposition = transform.position;

            Vector2 dir = mouse - playerposition;

            float angle = Vector2.SignedAngle(Vector2.right, dir);

            if (-45f < angle && angle <= 45f)
            {
                
                offset.x = 1;
                offset.y = 0;
            }
            else if (45f < angle && angle <= 135f)
            {
                offset.x = 0;
                offset.y = 1;
            }
            else if (135f < angle || angle <= -135f)
            {
                offset.x = -1;
                offset.y = 0;
            }
            else if (-135f < angle && angle <= -45f)
            {
                offset.x = 0;
                offset.y = -1;
            }

            Vector3Int coordinate = wall.WorldToCell(playerposition);
            Debug.Log(coordinate);

            wallspriterTransform.position = wall.CellToWorld(coordinate+offset)+tilemapCoordOffset;

            yield return new WaitForSeconds(0.1f);
        }
    }

    [PunRPC]
    public void DigOrBuild(Vector2 mouse, Vector2 playerposition)
    {
        DigAnim();
        Vector2 dir = mouse - playerposition;

        wall = GameObject.Find("Wall").GetComponent<Tilemap>();


        float angle = Vector2.SignedAngle(Vector2.right, dir);
               

        if (-45f <angle && angle <= 45f)
        {
            offset.x = 1;
            offset.y = 0;
        }
        else if(45f < angle && angle <= 135f)
        {
            offset.x = 0;
            offset.y = 1;
        }else if (135f < angle || angle <= -135f)
        {
            offset.x = -1;
            offset.y = 0;
        }
        else if (-135f < angle && angle <= -45f)
        {
            offset.x = 0;
            offset.y = -1;
        }

        coord = wall.WorldToCell(playerposition);
        tile = wall.GetTile<Tile>(coord+offset);

    }

    public void DigDone()
    {
        if (DigOrBuildBool)
        {
            if (tile == hit0)
            {
                wall.SetTile(coord + offset, hit1);
            }
            else if (tile == hit1)
            {
                wall.SetTile(coord + offset, hit2);
            }
            else if (tile == hit2)
            {
                wall.SetTile(coord + offset, null);

                if (Random.Range(0f, 1f) > 0.1)
                {
                    num_rocks++;
                }
            }
        }
        else
        {
            if (tile == null)
            {
                wall.SetTile(coord + offset, hit0);
                num_rocks--;
            }
        }
        
    }
    




}
