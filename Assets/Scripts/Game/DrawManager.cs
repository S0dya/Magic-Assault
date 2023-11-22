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
    public float arrowTresholdDistance;

    public float manaUsageOfSpellDrawnWrong;

    public float reloadingSpeed;

    [Header("SerializeFields")]
    [SerializeField] SpellsManager spellsManager;
    [SerializeField] Camera cam;
    [SerializeField] LineRenderer lineRenderer;
    Player player;

    //local
    [HideInInspector] public bool isOnUI;

    [HideInInspector] public bool inputChecked;
    bool isInput;

    int touchesCount;

    List<Vector2> drawPoints = new List<Vector2>();
    int positionCount = 0;

    bool canCreateSpell = true;

    //cors
    Coroutine checkJoystickInputCor;
    Coroutine changeSizeOfLineCor;
    Coroutine reloadingCor;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (isOnUI) return;

        touchesCount = Input.touchCount;

        //check if there is no input and if input is not related to joystick
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
                RecogniseShape();

                StopCreatingSpell();

                //reload spells ()
                if (reloadingCor != null) StopCoroutine(reloadingCor);
                reloadingCor = StartCoroutine(ReloadingCor());
            }
        }
    }

    IEnumerator CheckJoystickInputCor()
    {
        yield return null;

        inputChecked = true;
        if (!player.joystickInput && touchesCount == 1)
        {
            isInput = true;
            changeSizeOfLineCor = StartCoroutine(ChangeSizeOfLineCor());
            DrawLine(cam.ScreenToWorldPoint(Input.GetTouch(0).position));
        }

        checkJoystickInputCor = null;
    }

    IEnumerator ReloadingCor()
    {
        canCreateSpell = false;
        yield return new WaitForSeconds(reloadingSpeed);
        canCreateSpell = true;
    }

    public void StopCreatingSpell()// we also need this method for UI scripts 
    {
        isInput = false;
        inputChecked = false;
        ClearLine();
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
        //punish player for not waiting for a reload
        if (!canCreateSpell)
        {
            PunishPlayer();
            return;
        }

        //set points for spell manager
        spellsManager.drawPoints = drawPoints;
        //set size according to length of line (adding 0.5f to make max size = 1)
        spellsManager.size = lineWidth[0] + 0.5f;
        //check dot
        if (positionCount < 3)
        {
            spellsManager.useDot();
            return;
        }

        float totalAngle = 0;
        float maxAngle = 0;
        Vector2 maxAnglePosition = new Vector2();

        //check all angles
        for (int i = 2; i < positionCount; i++)
        {
            Vector2 prevPos = drawPoints[i - 1] - drawPoints[i - 2];
            Vector2 curPos = drawPoints[i] - drawPoints[i - 1];

            //get current angle and add it to tatal amount of angles of this shape
            float angle = Vector2.Angle(prevPos, curPos);
            totalAngle += angle;

            //angle of 360 can be reached with square shape
            if (maxAngle < angle)
            {
                //arrow's totalAngle can work even if we dont have an angle that is nearly equal to 90(half of a circle for example)
                maxAngle = angle;
                //we need to find position of max angle for arrow, to instantiate spell correctly
                maxAnglePosition = drawPoints[i];
            }
        }

        // Check for shapes based on total angle and treshold
        if (Mathf.Abs(totalAngle - 360.0f) < angleThreshold && maxAngle < 80 
            && Vector2.Distance(drawPoints[0], drawPoints[^1]) < circleTresholdDistance)//since S shape can still have suitable angle, we check distance of first and last points
        {
            //size of water is min between distance and set value
            float distance = Vector2.Distance(drawPoints[0], drawPoints[positionCount / 2]);
            spellsManager.useCircle(Mathf.Min(distance, Settings.maxSizeOfCircleSpell));
        }
        else if (totalAngle < angleThreshold)
        {
            spellsManager.useLine();
        }
        else if (Mathf.Abs(totalAngle - 150) < angleThreshold && maxAngle > 80
            && Vector2.Distance(drawPoints[0], drawPoints[^1]) > arrowTresholdDistance)//avoid player drawing beggining and last dots too close to each other
        {
            spellsManager.useArrow(maxAnglePosition);
        }
        else
        {
            PunishPlayer();
        }
    }

    void PunishPlayer() => player.ChangeMana(manaUsageOfSpellDrawnWrong);

    void ClearLine()
    {
        if (changeSizeOfLineCor != null) StopCoroutine(changeSizeOfLineCor);

        drawPoints.Clear();
        lineRenderer.positionCount = positionCount = 0;
    }
}
