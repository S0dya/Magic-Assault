using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform playerTransform;

    [Header("Settings")]
    public float damage;

    public float hp;
    public float movementSpeed;

    public float durationOfBurning;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

    //local
    bool canMove = true;
    bool isBurning;
    bool isPushed;
    bool isWet;

    float burningTime;
    float takingDamageVisualisationTime;

    //cors
    Coroutine burningCor;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        takingDamageVisualisationTime = Settings.takingDamageVisualisationTime;
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

    //health
    public void ChangeHP(float val)
    {
        float newHp = hp + val;
        if (val > 0) hp = Mathf.Min(newHp, 100);
        else
        {
            StartCoroutine(VisualiseDamage());
            hp = Mathf.Max(0, newHp);
        }

        if (hp == 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator VisualiseDamage()
    {
        sr.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(takingDamageVisualisationTime);
        sr.color = new Color(255, 255, 255);
    }

    //burning
    public void StartBurning(float damage)
    {
        burningTime = 0;
        if (isBurning) return;
        isBurning = true;
        
        burningCor = StartCoroutine(BurningCor(damage));
    }
    IEnumerator BurningCor(float damage)
    {
        while (burningTime < durationOfBurning)
        {
            burningTime++;
            ChangeHP(damage);

            yield return new WaitForSeconds(2f);
        }

        StopBurning();
    }
    public void StopBurning()
    {
        if (burningCor != null) StopCoroutine(burningCor);
        isBurning = false;
    }
}
