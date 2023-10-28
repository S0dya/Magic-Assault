using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [Header("Settings")]
    public float mocementSpeed;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Joystick joystick;


    //local 
    bool joystickInput;

    protected override void Awake()
    {
        base.Awake();

    }

    void Update()
    {
        if (joystickInput)
        {
            rb.velocity = joystick.Direction * mocementSpeed;
        }
    }


    //joystick input
    public void ToggleJoystickInput(bool val)
    {
        joystickInput = val;
        if (!val) rb.velocity = Vector2.zero;
    }
}
