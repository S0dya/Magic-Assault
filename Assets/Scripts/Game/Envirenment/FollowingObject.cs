using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LeanTween;

public class FollowingObject : MonoBehaviour
{
    public float expOnCollect;

    [SerializeField] Rigidbody2D rb;
    Transform playerTransform;


    void Awake()
    {
        transform.localScale = Vector2.zero;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        LeanTween.scale(gameObject, new Vector2(1, 1), 1).setEase(LeanTweenType.easeOutBack);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FollowPlayerCor());
    }

    IEnumerator FollowPlayerCor()
    {
        while (true)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * 14;

            if (Vector2.Distance(transform.position, playerTransform.position) < 0.25f) break;
            yield return null;
        }

        UIInGame.I.ChangeExp(expOnCollect);
        Destroy(gameObject);
    }
}
