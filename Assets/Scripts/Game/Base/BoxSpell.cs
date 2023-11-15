using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpell : Spell
{
    public BoxCollider2D boxCol;
    protected virtual void Start() => boxCol.size = new Vector2(size, size);
}
