using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawManager : SingletonMonobehaviour<DrawManager>
{
    Inputs inputs;

    //local
    Vector2 touchStartPos;
    bool isInput;

    protected override void Awake()
    {
        base.Awake();

        inputs = new Inputs();
    }
    void OnEnable()
    {
        inputs.Enable();
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void Start()
    {
        inputs.Touch.Touch.started += context => StartTouch(context);
        inputs.Touch.Touch.canceled += context => EndTouch(context);
    }

    void Update()
    {
        if (isInput)
        {
            if (Input.touchCount > 0)
            {
                Debug.Log("asd");
            }
            else if (Input.GetMouseButton(0))
            {
                Debug.Log("123");
            }
        }
    }

    //input
    void StartTouch(InputAction.CallbackContext context)
    {
        touchStartPos = context.ReadValue<Vector2>();
        isInput = true;

    }
    void EndTouch(InputAction.CallbackContext context)
    {
        isInput = false;
        touchStartPos = Vector2.zero;
    }
}
