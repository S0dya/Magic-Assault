using UnityEngine;

public class Magnet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.I.MagnetExp();
        Destroy(gameObject);
    }
}
