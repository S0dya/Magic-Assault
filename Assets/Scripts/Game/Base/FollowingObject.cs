using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LeanTween;

public class FollowingObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SO_Item thisItem;

    //local
    Transform playerTransform;

    protected virtual void Start()//find player this object will follow on trigger enter
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        LeanTween.scale(gameObject, new Vector2(1, 1), 1).setEase(LeanTweenType.easeOutBack);//make object appear 
    }

    protected virtual IEnumerator FollowPlayerCor()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 playerPos;
        Vector2 thisPos;

        while (distance > 0.25f)//follow player 
        {
            playerPos = playerTransform.position;
            thisPos = transform.position;

            Vector2 direction = (playerPos - thisPos).normalized;
            rb.velocity = direction * 10;

            distance = Vector2.Distance(thisPos, playerPos);
            yield return null;
        }

        UIResults.I.AddPickableItem(thisItem);
    }

    public Player GetPlayer()
    {
        return playerTransform.gameObject.GetComponent<Player>();
    }
}
