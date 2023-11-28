using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LeanTween;

public class FollowingObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Transform playerTransform;


    protected virtual void Awake()
    {
        //transform.localScale = Vector2.zero;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        LeanTween.scale(gameObject, new Vector2(1, 1), 1).setEase(LeanTweenType.easeOutBack);
    }

    protected virtual IEnumerator FollowPlayerCor()
    {
        while (true)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * 14;

            if (Vector2.Distance(transform.position, playerTransform.position) < 0.25f) break;
            yield return null;
        }

        Destroy(gameObject);
    }
}
