using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Settings")]
    public float distanceForFire;

    [Header("SerializeFields")]
    [SerializeField] DrawManager drawManager;
    [SerializeField] GameObject fireEffect;

    [SerializeField] Transform effectsParent;

    [HideInInspector] public List<Vector2> drawPoints;

    protected override void Awake()
    {
        base.Awake();


    }


    //Spells
    public void useDot()
    {

        Debug.Log("dot detected");

    }

    public void useCircle()
    {
        Debug.Log("Circle detected");

    }

    public void useArrow()
    {
        Debug.Log("Arrow detected");

    }

    public void useLine()
    {
        float distance = Vector2.Distance(drawPoints[0], drawPoints[^1]);
        float numObjects = Mathf.CeilToInt(distance / distanceForFire);

        for (int i = 0; i < numObjects; i++)
        {
            var obj = InstantiateEffect(fireEffect, Vector2.Lerp(drawPoints[0], drawPoints[^1], i/numObjects));
        }
    }

    GameObject InstantiateEffect(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity, effectsParent);
    }
}
