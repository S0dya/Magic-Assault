using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollForMultiplierUpgrade : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        UIInGame.I.OpenMultipliersPanel();
        Destroy(gameObject);
    }
}
