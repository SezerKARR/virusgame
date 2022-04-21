using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public enum side
{
    player = 0,
    enemyRed = 1,
    enemyYellow = 2,
    unside = 3,
};
public enum boxPointSize
{
    clear = 0,
    little = 20,
    middle = 30,
    large = 50,
    veryLarge = 80,
    holyLarge = 100
};
public enum unbiasedBox
{
    clear = 0,
    little = 3,
    middle = 6,
    large = 10,
    veryLarge = 13,
    holyLarge = 26,
    bestLarge = 33,
};
public enum kind
{
    normal,
    atacker,
    healer
}
public class Box : MonoBehaviour
{
    public TextMeshPro textNeedPoint;
    public Sprite[] spritesKind = new Sprite[3];
    public kind kind;
    public unbiasedBox unbiased;
    public GameObject pointTextPrefab;
    public side nameColor;
    public boxPointSize size;
    public GameObject circleLine;
    public GameObject cuttedLinePrefab;
    private int pointForLine = 10;
    public float pointBox;
    public int howManyLine = 3;
    public float maxPointBox;
    public TextMeshPro textPointBox;
    public EnemyAI enemyAI;
    private SpriteRenderer spriteRenderer;
    private GameObject[] circles = new GameObject[3];
    private int haveLine;
    private float[] decreases = new float[3];
    private float increase;
    private float maxDecrease;
    private int posMaxDecrease = 0;
    private float timeMove = 0;
    [HideInInspector]
    public GameObject[] lines = new GameObject[3];
    
    // Start is called before the first frame update
    void Awake()
    {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        maxPointBox = 100;
        if (nameColor == side.player)
        {
            gameObject.tag = "Player";
            spriteRenderer.sprite = spritesKind[kind.GetHashCode()];
            spriteRenderer.color = Color.green;
        }
        else if (nameColor == side.enemyRed)
        {
            gameObject.tag = "EnemyRed";
            spriteRenderer.sprite = spritesKind[kind.GetHashCode()];
            spriteRenderer.color = Color.red;
        }
        else if (nameColor == side.enemyYellow)
        {
            gameObject.tag = "EnemyYellow";
            spriteRenderer.sprite = spritesKind[kind.GetHashCode()];
            spriteRenderer.color = Color.yellow;
        }
        if (pointBox == 0)
            pointBox = size.GetHashCode();
        if (nameColor == side.unside)
        {
            gameObject.tag = "Unside";
            spriteRenderer.sprite = spritesKind[kind.GetHashCode()];
            textNeedPoint.gameObject.SetActive(true);
            textNeedPoint.text = "" + unbiased.GetHashCode();
        }
        enemyAI = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyAI>();

    }
    private void ColorChanged()
    {




        enemyAI.Start();

        if (nameColor.GetHashCode() < 3)
        {
            increase += decreases[nameColor.GetHashCode()];
            decreases[nameColor.GetHashCode()] = 0;
        }

    }
    public float getAtactPoint()
    {
        switch (kind)
        {
            case kind.normal:
                return 1f;
            case kind.healer:
                return 1f;

            case kind.atacker:
                return 2f;



        }
        return 1f;
    }
    public float getHealPoint()
    {
        switch (kind)
        {
            case kind.normal:
                return 1f;
            case kind.atacker:
                return 1f;
            case kind.healer:
                return 2f;
        }
        return 1f;
    }
    public float getLineDistance(int linePos)
    {
        return lines[linePos].GetComponent<Line>().GetDistance();
    }
    public GameObject GetLine(int whichLine)
    {
        if (whichLine > -1)
            return lines[whichLine];
        return null;
    }
    public void SetLine(GameObject line)
    {
        if (haveLine == howManyLine)
        {
            Destroy(line);
        }
        else if (haveLine < howManyLine)
        {
            lines[haveLine] = line;
            circles[haveLine].GetComponent<SpriteRenderer>().color = Color.blue;
            haveLine++;
        }



    }

    public float GetCanLineFloat()
    {

        return Mathf.Ceil(pointBox) - ((1 + haveLine) * pointForLine);
    }

    public int getLinesPosition(Vector2 finishPos)
    {

        for (int i = 0; i <= haveLine - 1; i++)
        {


            if (lines[i] != null)
            {
                Vector2 startPos = lines[i].GetComponent<Line>().GetObject(1).transform.position;

                if (startPos == finishPos)
                {

                    return i;
                }
            }


        }

        return -1;

    }
    public void CreateCuttedLine(Vector2 startPosition, GameObject target)
    {

        GameObject currentTrail = Instantiate(cuttedLinePrefab, Vector3.zero, Quaternion.identity);
        currentTrail.GetComponent<CuttedLine>().setPosition(startPosition, target);
    }


    public void DestroyLine(int whichLine, Vector2 cuttedPos, int createCuttedLine)
    {
        if (whichLine > -1)
        {
            Debug.Log(createCuttedLine);
            switch (createCuttedLine)
            {
                
                case 0:
                    CreateCuttedLine(lines[whichLine].GetComponent<LineRenderer>().GetPosition(1), this.gameObject);
                    increase = increase + Vector2.Distance(cuttedPos,
                    this.gameObject.transform.position)*2;
                    Destroy(lines[whichLine]);
                    haveLine--;
                    circles[haveLine].GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 1:
                    increase = increase + Vector2.Distance(lines[whichLine].GetComponent<LineRenderer>().GetPosition(1), cuttedPos) ;

                    lines[whichLine].GetComponent<Line>().SetPosition(cuttedPos, true);
                    break;
                case 2:
                    CreateCuttedLine(lines[whichLine].GetComponent<LineRenderer>().GetPosition(1), this.gameObject);
                    increase = increase + Vector2.Distance(cuttedPos,
                    this.gameObject.transform.position) * 2;
                    Destroy(lines[whichLine]);
                    haveLine--;
                    circles[haveLine].GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 3:
                    CreateCuttedLine(lines[whichLine].GetComponent<LineRenderer>().GetPosition(1), this.gameObject);
                    increase = increase + lines[whichLine].GetComponent<Line>().GetDistance()*4;

                    Destroy(lines[whichLine]);
                    haveLine--;
                    circles[haveLine].GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 4:

                    lines[whichLine].GetComponent<Line>().SetPosition(cuttedPos, false);
                    break;
                case 5:
                    increase = increase + Vector2.Distance(lines[whichLine].GetComponent<LineRenderer>().GetPosition(1), cuttedPos);

                    break;


            }
        }


    }
    public bool GetCanLine()
    {

        if (haveLine < howManyLine)
            return true;
        return false;
    }
    public float GetBoxPoint()
    {
        return pointBox;
    }
    // Update is called once per frame
    void Update()
    {
        if (haveLine < 0)
        {
            haveLine = 0;
        }

        if (nameColor == side.unside)
        {


            if (pointBox > unbiased.GetHashCode())
            {


                pointBox = pointBox + (unbiased.GetHashCode() / 5);
                nameColor = (side)posMaxDecrease;
                Destroy(textNeedPoint);
                Awake();
                ColorChanged();

            }
            if (posMaxDecrease > -1)
            {
                health(posMaxDecrease);
            }

        }

        else if (nameColor != side.unside)
        {
            if (pointBox < 0)
            {

                nameColor = (side)posMaxDecrease;
                Awake();
                ColorChanged();
            }
            health(nameColor.GetHashCode());
            //pointBox = pointBox +(pointBox* 0.0002f);



            if (pointBox > maxPointBox)
                pointBox = maxPointBox;
            howManyLine = (int)(Mathf.Ceil(pointBox) / pointForLine);



            if (howManyLine > 3)
                howManyLine = 3;


            for (int x = 0; x < howManyLine; x++)
            {

                if (circles[x] == null)
                {
                    circles[x] = Instantiate(circleLine, gameObject.transform.position - new Vector3(0.2f, 0.2f, 0f) +
                        new Vector3(x * 0.2f, 0f, 0f), Quaternion.identity, gameObject.transform);

                }


            }
            for (int x = 2; howManyLine - 1 < x; x--)
            {

                if (circles[x] != null)
                {
                    if (lines[x] != null)
                    {
                        lines[x].GetComponent<Line>().DestroyLane(false);
                        
                    }

                    Destroy(circles[x]);



                }

            }

        }

        timeMove += Time.deltaTime;

        if (timeMove >= 2f)
        {
            if ((nameColor == side.enemyYellow || nameColor == side.enemyRed) && GetCanLine())
            {


                enemyAI.Movement();
                timeMove = 0f;

            }
        }


        textPointBox.text = "" + Mathf.Ceil(pointBox);

    }
    public void SetPointDecrease(float lineDistance, GameObject atackingBox)
    {

        switch (atackingBox.tag)
        {
            case "Player":
                decreases[0] = decreases[0] + lineDistance;
                break;
            case "EnemyRed":
                decreases[1] = decreases[1] + lineDistance;
                break;
            case "EnemyYellow":
                decreases[2] = decreases[2] + lineDistance;
                break;

        }
        if (pointBox < 0)
        {
            for (int i = 0; i < decreases.Length; i++)
            {

                if (maxDecrease < decreases[i] && i != nameColor.GetHashCode())
                {

                    maxDecrease = decreases[i];
                    posMaxDecrease = i;
                }
            }
        }



    }
    void health(int deger)
    {



        for (int i = 0; i < decreases.Length; i++)
        {

            if (decreases[i] > Decrease(i))
            {
                if (i == deger && nameColor == side.unside)
                {

                    pointBox = pointBox + Decrease(i);
                    decreases[i] = decreases[i] - Decrease(i);

                }
                else
                {

                    pointBox = pointBox - Decrease(i);
                    decreases[i] = decreases[i] - Decrease(i);

                }
            }

        }
        if (increase > 0.01f)
        {

            pointBox = pointBox + increase * 0.02f;

            increase -= increase * 0.02f;
        }



    }
    private float Decrease(int arrayPos)
    {
        return decreases[arrayPos] * 0.02f + 0.02f;
    }


    public void SetPointIncrease(float lineDistance)
    {

        increase += lineDistance;
    }
}
