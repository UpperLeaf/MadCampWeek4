using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Tilemap wall;

    private Animator animator;
    private Animator weaponAnimator;
    private Animator armAnimator;
    // Start is called before the first frame update
    void Start()
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            Debug.Log(tilemap.tag);
            if (tilemap.CompareTag("wall"))
            {
                wall = tilemap;
            }
        }


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

        

        wall.SetTile(wall.WorldToCell(playerposition) + offset, null);

    }
}
