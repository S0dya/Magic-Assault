using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestForSpellChoose : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        UIInGame.I.OpenSpellsPanel();
        Destroy(gameObject);
    }
}
