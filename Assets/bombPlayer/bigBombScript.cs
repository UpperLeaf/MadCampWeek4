﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigBombScript : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D shakeDistance;

    private Vector3 onAirVelocity = new Vector3(0, 0, 0);

    private float bombSpeed = 5f;

    private float bombheight = 3f;

    private float acceleration;

    private float Distance;

    public Vector2 startposition;

    public Vector2 Target;

    private Vector3 displacement;

    private bool blowup = false;

    public float damage;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip bombAttackClip;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = startposition;

        Distance = Vector2.Distance(startposition, Target);

        //for far target
        acceleration = -8 * bombheight * bombSpeed * bombSpeed / (Distance * Distance);
        displacement = new Vector3((Target - startposition).normalized.x, (Target - startposition).normalized.y, 0);
        onAirVelocity.y = 4 * bombSpeed * bombheight / Distance;

        audioSource = GetComponent<AudioSource>();

        //for near target
        if (Distance <= 3f)
        {
            Debug.Log("near");
            acceleration = -60f;
            onAirVelocity.y = -acceleration * Distance / (2 * bombSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(Target, transform.position) > 0.3f)
        {
            transform.position += bombSpeed * Time.deltaTime * displacement + onAirVelocity * Time.deltaTime;
            onAirVelocity.y += acceleration * Time.deltaTime;
        }
        else
        {
            GetComponent<Animator>().SetTrigger("blowUp");
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void blowUp()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = 0.7f;
        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, 6 * collider.radius);

        foreach (Collider2D _collider in overlappedColliders)
        {
            if (_collider.CompareTag("Player"))
            {
                _collider.gameObject.GetComponent<PlayerManager>().Damaged(damage);
            }
        }

        Collider2D[] shakeDistancePlayerColliders = Physics2D.OverlapCircleAll(transform.position, 6 * shakeDistance.radius);
        audioSource.PlayOneShot(bombAttackClip);

        foreach (Collider2D _collider in shakeDistancePlayerColliders)
        {
            if (_collider.CompareTag("Player"))
            {
                _collider.GetComponent<AbstractPlayerScript>().ShakeCameraAttack(5f, 0.4f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            GetComponent<Animator>().SetTrigger("blowUp");
        }
    }
}
