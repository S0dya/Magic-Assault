using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [Header("Settings")]
    public float mocementSpeed;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody2D rb;
    [Header("UI")]
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;
    Joystick joystick;


    //local 
    [HideInInspector] public bool joystickInput;

    protected override void Awake()
    {
        base.Awake();

        SetJoystick();
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
        if (!val)
        {
            rb.velocity = Vector2.zero;
            //skip this frame for proper value set
            StartCoroutine(UnSetCheckCor());
        }
    }
    IEnumerator UnSetCheckCor()
    {
        yield return null;

        DrawManager.I.inputChecked = false;
    }

    //other methods
    void SetJoystick()
    {
        bool isFJ = Settings.isFloatingJoystick;

        if (isFJ) flJoystick.gameObject.SetActive(true);
        else fxJoystick.gameObject.SetActive(true);

        joystick = isFJ ? flJoystick : fxJoystick;
    }
}
