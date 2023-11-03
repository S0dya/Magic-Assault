using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Settings")]
    public float distanceForFire;

    [Header("SerializeFields")]
    [SerializeField] Player player;
    Transform playerTransform;
    [SerializeField] DrawManager drawManager;
    //dot
    [SerializeField] GameObject stoneEffect;
    //line
    [SerializeField] GameObject fireEffect;
    //circle
    [SerializeField] GameObject waterEffect;
    //arrow
    [SerializeField] GameObject windEffect;

    [SerializeField] Transform effectsParent;


    [HideInInspector] public List<Vector2> drawPoints;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = player.transform;
    }


    //Spells
    public void useDot(float size)
    {
        var obj = InstantiateEffect(stoneEffect, playerTransform.position);
        var stone = obj.GetComponent<Stone>();
        stone.size = size;

        Vector2 direction = (drawPoints[^1] - (Vector2)playerTransform.position).normalized;
        float angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;
        stone.angle = angleInDegrees;
    }

    public void useCircle(float size)
    {
        var obj = InstantiateEffect(waterEffect, Vector2.Lerp(drawPoints[0], drawPoints[drawPoints.Count/2], 0.5f));
        var water = obj.GetComponent<Water>();
        water.size = size;
    }

    public void useArrow(float size, Vector2 middleElementPos)
    {
        Debug.Log("Arrow detected");

        var obj = InstantiateEffect(windEffect, );

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
