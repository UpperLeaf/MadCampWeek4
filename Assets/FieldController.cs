using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
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
        StartCoroutine("FieldResize");
        StartCoroutine("DamageToPlayer");

    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, BattleFieldNotifier.transform.position, circlespeed*Time.deltaTime);
    }

    IEnumerator FieldResize()
    {
        for (int i = 0; i < 4; i++)
        {
            float r = UnityEngine.Random.Range(0f, StageScale[currentBattlefieldStage] - StageScale[currentBattlefieldStage + 1]);
            float theta = UnityEngine.Random.Range(-180f, 180f);

            Vector3 offset = new Vector3(r * Mathf.Cos(theta) * fieldCollider.radius, r * Mathf.Sin(theta) * fieldCollider.radius, 0);
            Debug.Log(r);
            Debug.Log(theta);
            Debug.Log(offset);


            BattleFieldNotifier.transform.position = transform.position + offset;
            BattleFieldNotifier.transform.localScale = new Vector3(StageScale[currentBattlefieldStage + 1], StageScale[currentBattlefieldStage + 1], 0);

            while (localScale.x > StageScale[currentBattlefieldStage + 1])
            {
                float size = localScale.x - 0.0005f;
                localScale = new Vector3(size, size, 1);
                transform.localScale = localScale;
                yield return new WaitForSeconds(0.02f);
            }

            currentBattlefieldStage++;
        }       
        
    }

    IEnumerator DamageToPlayer()
    {
        while (true)
        {
            Vector3 position = transform.position - colliderOffset;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, fieldCollider.radius * localScale.x);
            
            GameManager.playerManagers.FindAll((playerManager) =>
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if (playerManager.gameObject.Equals(colliders[i].gameObject))
                    {
                        return false;
                    }
                }
                return true;
            }).ForEach((manager) =>
            {
                Debug.Log("Manager : " + manager);
                manager.GetComponent<PlayerManager>().Damaged(0.01f);
            });

            yield return new WaitForSeconds(1f);
        }
    }
}
