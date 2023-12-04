using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.I.MagnitExp();
        Destroy(gameObject);
    }
}
