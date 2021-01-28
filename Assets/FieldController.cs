using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviourPunCallbacks
{
    private CircleCollider2D fieldCollider;

    [SerializeField]
    private GameObject BattleFieldNotifier;
    
    private Vector3 localScale;
    private Vector3 colliderOffset;

    private int currentBattlefieldStage = 0;
    private float[] StageScale;

    private float circlespeed = 1f;

    void Start()
    {
        StageScale = new float[] {2.0f, 1.5f, 1.0f, 0.5f, 0.2f };
        fieldCollider = GetComponent<CircleCollider2D>();
        colliderOffset = fieldCollider.offset;
        localScale = transform.localScale;

        Invoke("Init", 3f);
    }

    private void Init()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine("FieldResize");
        
        StartCoroutine("DamageToPlayer");
    }

    IEnumerator FieldResize()
    {
        for (int i = 0; i < 4; i++)
        {
            float r = UnityEngine.Random.Range(0f, StageScale[currentBattlefieldStage] - StageScale[currentBattlefieldStage + 1]);
            float theta = UnityEngine.Random.Range(-180f, 180f);

            Vector3 offset = new Vector3(r * Mathf.Cos(theta) * fieldCollider.radius, r * Mathf.Sin(theta) * fieldCollider.radius, 0);

            BattleFieldNotifier.transform.position = transform.position + offset;
            BattleFieldNotifier.transform.localScale = new Vector3(StageScale[currentBattlefieldStage + 1], StageScale[currentBattlefieldStage + 1], 0);

            int max = 2000;
            float distx = offset.x / max ;
            float disty = offset.y / max ;
            float diff = (StageScale[currentBattlefieldStage] - StageScale[currentBattlefieldStage + 1]) / max;
            int idx = 0;
            while (idx < max)
            {
                localScale.x -= diff;
                localScale.y -= diff;
                transform.localScale = localScale;

                transform.Translate(distx, disty, 0);
                photonView.RPC("ResizeClient", PhotonTargets.All, new object[] {transform.position, transform.localScale });
                idx++;
                yield return new WaitForSeconds(0.02f);
            }
            currentBattlefieldStage++;
        }       
        
    }

    [PunRPC]
    void ResizeClient(Vector3 position, Vector3 scale)
    {
        transform.position = position;
        transform.localScale = scale;
    }

    IEnumerator DamageToPlayer()
    {
        while (true)
        {
            Vector3 position = transform.position - colliderOffset;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, fieldCollider.radius * transform.localScale.x);
            Debug.Log("PlayerManagers : "  + GameManager.playerManagers);
            GameManager.playerManagers.FindAll((playerManager) =>
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if (playerManager.gameObject.Equals(colliders[i].gameObject))
                    {
                        Debug.Log("In Battle Field Player" + playerManager);
                        return false;
                    }
                }
                Debug.Log("Out Battle Field Player" + playerManager);
                return true;
            }).ForEach((manager) =>
            {
                Debug.Log("Damaged Player : " + manager);
                manager.GetComponent<PlayerManager>().Damaged(0.05f);
            });

            yield return new WaitForSeconds(0.5f);
        }
    }
}
