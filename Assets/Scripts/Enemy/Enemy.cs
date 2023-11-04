using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform playerTransform;
    
    [Header("Settings")]
    public float movementSpeed;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody2D rb;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * movementSpeed;
    }
}
