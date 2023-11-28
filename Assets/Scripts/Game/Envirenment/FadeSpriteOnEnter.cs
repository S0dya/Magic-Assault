using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSpriteOnEnter : MonoBehaviour
{
    [SerializeField] SpriteRenderer sp;

    void OnTriggerEnter2D(Collider2D collision)
    {
        sp.color = new Color(255, 255, 255, 0.6f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        sp.color = new Color(255, 255, 255, 1);
    }
}
