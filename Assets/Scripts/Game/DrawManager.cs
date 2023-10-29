using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : SingletonMonobehaviour<DrawManager>
{
    [Header("SerializeFields")]
    [SerializeField] Camera cam;
    [SerializeField] LineRenderer lineRenderer;


    //local
    [HideInInspector] public bool inputChecked;
    bool isInput;

    int touchesCount;

    List<Vector2> drawPoints = new List<Vector2>();
    int positionCount = 0;

    //cors
    Coroutine checkJoystickInputCor;

    protected override void Awake()
    {
        base.Awake();

    }

    void Update()
    {
        touchesCount = Input.touchCount;
        
        //check if we there is no input and if input is not related to joystick
        if (!inputChecked && touchesCount == 1 && checkJoystickInputCor == null)
        {
            checkJoystickInputCor = StartCoroutine(CheckJoystickInputCor());
        }

        //draw
        if (isInput)
        {
            if (touchesCount == 1)
            {
                Vector2 inputPos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                if (Vector2.Distance(inputPos, lineRenderer.GetPosition(positionCount - 1)) > 0.5f)
                {
                    DrawLine(inputPos);
                }
            }
            else if (touchesCount == 0 || touchesCount > 1)
            {
                isInput = false;
                inputChecked = false;
                RecogniseShape();
                ClearLine();
            }
        }
    }

    IEnumerator CheckJoystickInputCor()
    {
        yield return null;

        inputChecked = true;
        if (!Player.I.joystickInput && touchesCount == 1)
        {
            isInput = true;
            DrawLine(cam.ScreenToWorldPoint(Input.GetTouch(0).position));
        }

        checkJoystickInputCor = null;
    }

    //line methods

    void DrawLine(Vector2 pos)
    {
        drawPoints.Add(pos);

        lineRenderer.positionCount++;
        positionCount++; 
        lineRenderer.SetPosition(positionCount - 1, pos);
    }

    void RecogniseShape()
    {
        Debug.Log(IsCircle(drawPoints));
    }

    void ClearLine()
    {
        drawPoints.Clear();
        lineRenderer.positionCount = positionCount = 0;
    }

    bool IsCircle(List<Vector2> points)
    {
        if (points.Count < 3)
        {
            return false;
        }

        float totalDistance = 0f;
        Vector2 center = Vector2.zero;

        foreach (Vector2 point in points)
        {
            center += point;
        }

        center /= points.Count;

        foreach (Vector2 point in points)
        {
            totalDistance += Vector2.Distance(center, point);
        }

        float averageRadius = totalDistance / points.Count;

        return averageRadius < 0.1f;
    }
}
