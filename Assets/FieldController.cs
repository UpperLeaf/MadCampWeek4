using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    private CircleCollider2D fieldCollider;
    
    private Vector3 localScale;
    private Vector3 colliderOffset;

    void Start()
    {
        fieldCollider = GetComponent<CircleCollider2D>();
        colliderOffset = fieldCollider.offset;
        localScale = transform.localScale;
        StartCoroutine("FieldResize");
        StartCoroutine("DamageToPlayer");
    }
    IEnumerator FieldResize()
    {
        while (localScale.x > 0.2f)
        {
            float size = localScale.x - 0.0005f;
            localScale = new Vector3(size, size, 1);
            transform.localScale = localScale;
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator DamageToPlayer()
    {
        Vector3 position = transform.position - colliderOffset;

        while (true)
        {
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
