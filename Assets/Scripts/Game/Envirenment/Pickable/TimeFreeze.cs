using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.I.FreezeEnemies();
        Destroy(gameObject);
    }
}
