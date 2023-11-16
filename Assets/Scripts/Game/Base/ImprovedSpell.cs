using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedSpell : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Spell spell;
    [SerializeField] GameObject afterSpell;

    protected virtual void OnDisable() => Instantiate();

    //this method is called for circle effects of water and lava, also for dot effect of fire
    public void Instantiate()
    {
        SpellsManager.I.InstantiateEffect(afterSpell, transform.position, spell.size, spell.damage / 2);
    }

    //method is called for earth effects
    public void InstantiateWithForce(int n, float directionRadius)
    {
        float spellDirectionX = spell.direction.x;
        float spellDirectionY = spell.direction.y;

        for (int i = 0; i < n; i++)
        {
            Vector2 curDirection = new Vector2(Random.Range(spellDirectionX + directionRadius, spellDirectionX - directionRadius), 
                Random.Range(spellDirectionY + directionRadius, spellDirectionY - directionRadius));

            SpellsManager.I.InstantiateEffect(afterSpell, transform.position, spell.size, spell.damage / 2, curDirection, spell.rotation);
        }
    }
}
