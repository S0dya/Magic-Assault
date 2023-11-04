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

    //local
    bool canMove;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (canMove)
        {
            rb.velocity = (playerTransform.position - transform.position).normalized * movementSpeed;
        }
    }

    public void ToggleMovement(bool val)
    {
        canMove = val;
        if (!canMove) rb.velocity = Vector2.zero;
    }
}
