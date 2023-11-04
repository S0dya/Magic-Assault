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
    //circle
    [SerializeField] GameObject waterEffect;
    //line
    [SerializeField] GameObject fireEffect;

    //arrow
    [SerializeField] GameObject windEffect;

    [SerializeField] Transform effectsParent;

    //local
    [HideInInspector] public float size;
    [HideInInspector] public List<Vector2> drawPoints;
    GameObject curDotEffect;
    GameObject curCircleEffect;
    GameObject curLineEffect;
    GameObject curArrowEffect;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = player.transform;
    }

    void Start()
    {
        curDotEffect = stoneEffect;
        curCircleEffect = waterEffect;
        curLineEffect = fireEffect;
        curArrowEffect = windEffect;
    }


    //Spells
    public void useDot()
    {
        //get angle from player to draw position
        Vector2 direction = (drawPoints[^1] - (Vector2)playerTransform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;
        
        InstantiateEffect(curDotEffect, playerTransform.position, size, angle, 0);
    }

    public void useCircle(float distance)
    {
        //get radius of circle
        Vector2 pos = Vector2.Lerp(drawPoints[0], drawPoints[drawPoints.Count / 2], 0.5f);
        InstantiateEffect(curCircleEffect, pos, size * distance, 0, 0);
    }

    public void useLine()
    {
        float distance = Vector2.Distance(drawPoints[0], drawPoints[^1]);
        float numObjects = Mathf.CeilToInt(distance / distanceForFire);

        //instantiate effect from first draw position to last 
        for (int i = 0; i < numObjects; i++)
        {
            InstantiateEffect(curLineEffect, Vector2.Lerp(drawPoints[0], drawPoints[^1], i / numObjects), size, 0, 0);
        }
    }

    public void useArrow(Vector2 middleElementPos)
    {
        //get angle from first and last dots to dot with highest angle
        Vector2 arrowStartPos = Vector2.Lerp(drawPoints[0], drawPoints[^1], 0.5f);
        Vector2 direction = (middleElementPos - arrowStartPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        InstantiateEffect(windEffect, middleElementPos, size, 0, rotation);

        if (Settings.additionalEffects)
        {
            InstantiateEffect(windEffect, drawPoints[0], size, 0, rotation);
            InstantiateEffect(windEffect, drawPoints[^1], size, 0, rotation);
        }
    }
    void InstantiateEffect(GameObject prefab, Vector2 pos, float size, float angle, float rotation)
    {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, effectsParent);
        Spell spell = obj.GetComponent<Spell>();

        spell.SetSize(size);
        if (angle != 0) spell.SetAngle(angle);
        if (rotation != 0) spell.SetRotation(rotation);

        spell.Play();
    }
}
