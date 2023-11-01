using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : SingletonMonobehaviour<DrawManager>
{
    [Header("Settings")]
    public float lineLength;
    public float[] lineWidth; //1 - min 2 - max 3 - starting value for coroutine 4 - end value for coroutine

    public float angleThreshold;
    public float circleTresholdDistance;


    [Header("SerializeFields")]
    [SerializeField] SpellsManager spellsManager;
    [SerializeField] Camera cam;
    [SerializeField] LineRenderer lineRenderer;


    //local
    [HideInInspector] public bool inputChecked;
    bool isInput;

    float powerOfSpell;
    int touchesCount;

    List<Vector2> drawPoints = new List<Vector2>();
    int positionCount = 0;

    //cors
    Coroutine checkJoystickInputCor;
    Coroutine changeSizeOfLineCor;

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
                if (Vector2.Distance(inputPos, lineRenderer.GetPosition(positionCount - 1)) > lineLength)
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
            changeSizeOfLineCor = StartCoroutine(ChangeSizeOfLineCor());
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

    IEnumerator ChangeSizeOfLineCor()
    {
        powerOfSpell = 0.1f;
        lineWidth[0] = lineWidth[2];

        while (lineWidth[0] < lineWidth[1])
        {
            lineRenderer.startWidth = lineWidth[0];
            lineRenderer.endWidth = lineWidth[0];

            lineWidth[0] = Mathf.Lerp(lineWidth[0], lineWidth[3], 0.005f);

            yield return null;
        }

        changeSizeOfLineCor = null;
    }


    void RecogniseShape()
    {
        //check dot
        spellsManager.drawPoints = drawPoints;
        if (positionCount < 3) spellsManager.useDot();

        float totalAngle = 0;
        float maxAngle = 0;

        //check all angles
        for (int i = 2; i < positionCount; i++)
        {
            Vector2 prevPos = drawPoints[i - 1] - drawPoints[i - 2];
            Vector2 curPos = drawPoints[i] - drawPoints[i - 1];

            //get current angle and add it to tatal amount of angles of this shape
            float angle = Vector2.Angle(prevPos, curPos);
            totalAngle += angle;

            //angle of 360 can be reached with square shape
            //arrow's totalAngle can work even if we dont have an angle that is nearly equal to 90(half of a circle for example)
            maxAngle = Mathf.Max(maxAngle, angle);
        }

        // Check for shapes based on total angle and treshold
        if (Mathf.Abs(totalAngle - 360.0f) < angleThreshold && maxAngle < 80 
            && Vector2.Distance(drawPoints[0], drawPoints[^1]) < circleTresholdDistance)//since S shape can still have suitable angle, we check distance of first and last points
        {
            spellsManager.useCircle();
        }
        else if (totalAngle < angleThreshold)
        {
            spellsManager.useLine();
        }
        else if (Mathf.Abs(totalAngle - 150) < angleThreshold && maxAngle > 80)
        {
            spellsManager.useArrow();
        }
    }

    void ClearLine()
    {
        if (changeSizeOfLineCor != null) StopCoroutine(changeSizeOfLineCor);
        drawPoints.Clear();
        lineRenderer.positionCount = positionCount = 0;
    }

    
}
