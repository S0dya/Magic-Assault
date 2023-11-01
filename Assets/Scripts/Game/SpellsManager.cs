using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Settings")]


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
        Debug.Log("Line detected");

        Vector2 step = drawPoints[1] - drawPoints[0] / (drawPoints.Count - 1);
        float n = (int)Vector2.Distance(drawPoints[0], drawPoints[1]);

        Debug.Log(step + " step");
        Debug.Log(n + " n");
        for (int i = 0; i < n; i++)
        {
            Debug.Log("---- " + drawPoints[0] + step * i);
            var obj = InstantiateEffect(fireEffect, drawPoints[0] + step * i);
        }
    }

    GameObject InstantiateEffect(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity, effectsParent);
    }
}
