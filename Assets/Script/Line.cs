using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRepeat lineRepeat;
    private GameObject gameController;
    public List<Vector2> boxPosition;
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public float speed = 2f;
    private float distance;
    public List<Vector2> edgePosition;
    public Vector2[] edgePositions = new Vector2[2];
    public Box box;
    public Box box1;
    private GameObject[] cuteLineTarget = new GameObject[2];
    Vector3 chase;
    Vector3 finishLinePosition;
    bool doubleAttack;
    int whichLine;
    void Awake()
    {

        gameController = GameObject.FindGameObjectWithTag("GameController");
        lineRepeat = gameController.GetComponent<LineRepeat>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>();


    }
    public void SetPosition(GameObject startLine, GameObject finishLine)
    {
        box = startLine.GetComponent<Box>();
        box1 = finishLine.GetComponent<Box>();

        lineRenderer.SetColors(startLine.GetComponent<SpriteRenderer>().color, startLine.GetComponent<SpriteRenderer>().color);
        cuteLineTarget[0] = startLine;
        cuteLineTarget[1] = finishLine;
        lineRenderer.SetPosition(0, cuteLineTarget[0].transform.position);
        lineRenderer.SetPosition(1, cuteLineTarget[0].transform.position);
        edgePositions[0] = cuteLineTarget[0].transform.position;
        finishLinePosition = finishLine.transform.position;
        whichLine = box1.getLinesPosition(cuteLineTarget[0].transform.position);
        box.SetPointDecrease(Vector2.Distance(cuteLineTarget[0].transform.position, cuteLineTarget[1].transform.position)*2, cuteLineTarget[0]);
        if (whichLine > -1)
        {
            
            if (cuteLineTarget[0].tag == cuteLineTarget[1].tag)
            {

                box1.DestroyLine(whichLine, cuteLineTarget[0].transform.position, 0);


            }

        }


    }
    public float GetDistance()
    {
        return Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
    }
    public void SetPosition(Vector2 MidlePos, bool doubleAttack)
    {
       
        if (doubleAttack)
        {

           
            finishLinePosition = MidlePos;
            Debug.Log(Vector2.Distance(cuteLineTarget[1].transform.position, finishLinePosition));
            box.SetPointIncrease(Vector2.Distance(cuteLineTarget[1].transform.position, finishLinePosition)*2);
            
        }


        else if (!doubleAttack)
        {
            Debug.Log("geldi");
            box.SetPointIncrease(Vector2.Distance(finishLinePosition, cuteLineTarget[1].transform.position)*2);
            finishLinePosition = cuteLineTarget[1].transform.position;
            this.doubleAttack = false;

        }


    }
    public GameObject GetObject(int i)
    {

        return cuteLineTarget[i];
    }

    private void FixedUpdate()
    {
        whichLine = box1.getLinesPosition(cuteLineTarget[0].transform.position);

        if (whichLine > -1&&!this.doubleAttack)
        {
            if (cuteLineTarget[0].tag != cuteLineTarget[1].tag)
            {
                distance = Vector2.Distance(cuteLineTarget[0].transform.position, cuteLineTarget[1].transform.position);

                if (distance <= GetDistance() + box1.getLineDistance(whichLine))
                {
                    Vector2 cuttedPos = cuteLineTarget[0].transform.position +
                    (cuteLineTarget[1].transform.position - cuteLineTarget[0].transform.position) / 2;

                    //Vector2 midle= cuteLineTarget[0].transform.position + (cuteLineTarget[1].transform.position - cuteLineTarget[0].transform.position) / 2;
                    box1.DestroyLine(whichLine, cuttedPos, 5);
                    finishLinePosition = cuttedPos;
                    this.doubleAttack = true;
                }

            }

        }
        if (lineRenderer.GetPosition(1)==finishLinePosition)
        {
            if (cuteLineTarget[0].tag == cuteLineTarget[1].tag )
            {



                box.SetPointDecrease(box.GetBoxPoint() / 3000f, cuteLineTarget[0]);
                box1.SetPointIncrease(box.GetBoxPoint() / 3000f * box.getHealPoint());
                
            }
            else if (cuteLineTarget[0].tag != cuteLineTarget[1].tag)
            {



                box.SetPointDecrease(box.GetBoxPoint() / 3000f, cuteLineTarget[0]);
                box1.SetPointDecrease(box.GetBoxPoint() / 3000f * box.getAtactPoint(), cuteLineTarget[0]);
            }
            
        }
       
       
        if (lineRenderer.GetPosition(1) != finishLinePosition)
        {

            edgePositions[1] = lineRenderer.GetPosition(1);
            edgeCollider.points = edgePositions;
            chase = Vector3.MoveTowards(lineRenderer.GetPosition(1), finishLinePosition, .1f);
            lineRenderer.SetPosition(1, chase);
            
        }






    }
    public void OnMouseDrag()
    {
        DestroyLane(true);
    }
    public void DestroyLane(bool mouse)
    {
        if (!mouse||cuteLineTarget[0].GetComponent<SpriteRenderer>().color == Color.green)
        {

            if (!doubleAttack && box.getLinesPosition(cuteLineTarget[1].transform.position) > -1)
            {


                if (box.GetLine(box.getLinesPosition(cuteLineTarget[1].transform.position)).GetComponent<LineRenderer>().GetPosition(1) == cuteLineTarget[1].transform.position)
                {
                    Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (inputPos == null)
                    {
                        inputPos = box1.transform.position;
                    }
                    lineRepeat.CreateCuttedLine(inputPos, cuteLineTarget[0]);
                    if (cuteLineTarget[0].tag == cuteLineTarget[1].tag)
                    {

                        box1.SetPointIncrease(Vector2.Distance(inputPos , cuteLineTarget[1].transform.position)* box.getHealPoint() * 2);

                    }
                    else if (cuteLineTarget[0].tag != cuteLineTarget[1].tag)
                    {

                        box1.SetPointDecrease(Vector2.Distance(inputPos, cuteLineTarget[1].transform.position) * box.getAtactPoint()*2, cuteLineTarget[0]);

                    }


                    

                    box.DestroyLine(box.getLinesPosition(cuteLineTarget[1].transform.position), inputPos, 2);
                }
                else
                {
                    lineRepeat.CreateCuttedLine(lineRenderer.GetPosition(1), cuteLineTarget[0]);
                    box.DestroyLine(box.getLinesPosition(cuteLineTarget[1].transform.position), lineRenderer.GetPosition(1), 3);
                }

            }
            else if (doubleAttack)
            {

                box.DestroyLine(box.getLinesPosition(cuteLineTarget[1].transform.position), lineRenderer.GetPosition(1), 3);
                box1.DestroyLine(box1.getLinesPosition(cuteLineTarget[0].transform.position), lineRenderer.GetPosition(1), 4);


            }
        }

    }
}
