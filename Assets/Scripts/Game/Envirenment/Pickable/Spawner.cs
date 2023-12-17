using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spaghetti

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color normalColor;
    [SerializeField] Color damageColor;

    [Header("On destroy")]
    [SerializeField] GameObject[] itemPrefabs;

    //local
    Transform pickableTransform;

    //cors
    Coroutine visualiseDamageCor;

    void Awake()
    {
        pickableTransform = GameObject.FindGameObjectWithTag("PickUps").transform;
    }

    public void ChangeHP(float val)
    {
        maxHp += val;

        if (maxHp <= 0) Destroy(gameObject);

        VisualiseDamage();
    }

    void OnDestroy()
    {
        Instantiate(itemPrefabs[(Random.Range(0, itemPrefabs.Length))], transform.position, Quaternion.identity, pickableTransform);
    }

    void VisualiseDamage()
    {
        if (visualiseDamageCor == null) visualiseDamageCor = StartCoroutine(VisualiseDamageCor());
    }
    IEnumerator VisualiseDamageCor()
    {
        ChangeSrColor(damageColor);

        yield return new WaitForSeconds(0.15f);

        ChangeSrColor(normalColor);
        visualiseDamageCor = null;
    }
    void ChangeSrColor(Color color) => sr.color = color;
}
