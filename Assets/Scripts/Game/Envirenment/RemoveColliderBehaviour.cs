using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColliderBehaviour : MonoBehaviour
{
    [SerializeField] CircleCollider2D collider;
    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        rb.AddForce(Vector2.up * 0.1f, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (Mathf.Approximately(rb.velocity.magnitude, 0f))
        {
            DestroyComponents();
        }
        else Debug.Log(transform.position);
    }

    void DestroyComponents()
    {
        Destroy(collider);
        Destroy(rb);
        Destroy(this);
    }
}
