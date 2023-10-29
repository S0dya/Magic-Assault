using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawManager : SingletonMonobehaviour<DrawManager>
{
    Inputs inputs;

    [Header("SerializeFields")]
    [SerializeField] Camera cam;
    [SerializeField] LineRenderer lineRenderer;

    //local
    Vector2 touchStartPos;
    bool isInput;

    Touch touch;
    List<Vector2> drawPoints = new List<Vector2>();

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
            Vector2 inputPos = cam.ScreenToWorldPoint(touch.position);
            if (Vector2.Distance(inputPos, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) > 1f)
            {
                DrawLine(inputPos);
            }

            if (Input.GetMouseButton(0))
            {
                Debug.Log("123");
            }
            
        }
    }

    //input
    void StartTouch(InputAction.CallbackContext context)
    {
        touch = context.ReadValue<Touch>();
        Debug.Log("123");
        touchStartPos = cam.ScreenToWorldPoint(touch.position);
        isInput = true;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, touchStartPos);
    }
    void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("re");
        isInput = false;
        ClearLine();
    }

    //line methods

    void DrawLine(Vector2 pos)
    {
        drawPoints.Add(pos);

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1, pos);
    }

    void ClearLine()
    {
        drawPoints.Clear();
        lineRenderer.positionCount = 0;
    }
}
