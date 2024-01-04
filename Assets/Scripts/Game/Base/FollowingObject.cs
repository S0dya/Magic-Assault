using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LeanTween;
using UnityEngine.Events;

public class FollowingObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SO_Item thisItem;

    [SerializeField] UnityEvent actionOnPlayerReached;

    //local
    Transform playerTransform;

    bool isFollowing; 

    void Start()//find player this object will follow on trigger enter
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        LeanTween.scale(gameObject, new Vector2(1, 1), 0.2f).setEase(LeanTweenType.easeOutBack);//make object appear 
    }

    //trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFollowing) StartFollowingPlayer();
    }

    public void StartFollowingPlayer()
    {
        isFollowing = true;
        StartCoroutine(FollowPlayerCor());
    }

    //main methods
    IEnumerator FollowPlayerCor()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 playerPos;
        Vector2 thisPos;

        while (distance > 0.25f)//follow player 
        {
            playerPos = playerTransform.position;
            thisPos = transform.position;

            Vector2 direction = (playerPos - thisPos).normalized;
            rb.velocity = direction * 6;

            distance = Vector2.Distance(thisPos, playerPos);
            yield return null;
        }

        actionOnPlayerReached.Invoke();
        UIResults.I.AddPickableItem(thisItem);
        Destroy(gameObject);
    }

    //other methods
    public Player GetPlayer()
    {
        return playerTransform.gameObject.GetComponent<Player>();
    }
}
