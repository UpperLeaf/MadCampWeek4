using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingZoneScript : MonoBehaviour
{

    private CircleCollider2D circle;

    [SerializeField]
    private float healamount;

    [SerializeField]
    private float healtime;

    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        healamount = 0.1f;
        healtime = 1f;

        StartCoroutine("heal");

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    IEnumerator heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(healtime);
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, circle.radius);

            foreach (Collider2D collider in overlappedColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    collider.gameObject.GetComponent<PlayerManager>().heal(healamount);
                }
            }
        }        
    }


}
