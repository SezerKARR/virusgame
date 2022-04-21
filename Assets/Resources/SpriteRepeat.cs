using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteRepeat : MonoBehaviour
{
    public Square square;
    public GameObject squarePrefab;
    public GameObject currentSquare;
    private GameObject startBox;
    private GameObject finishBox;
    public Vector2 currentSquarePos;
    public GameObject linePrefab;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                if (startBox == null)
                {

                    startBox = hit.transform.gameObject;
                    startBox.GetComponent<SpriteRenderer>().color = Color.red;
                    
                    //startBox.transform.DOMove(new Vector3(1, 2, 3), 1);

                }
                else
                {
                    
                    finishBox = hit.transform.gameObject;
                    CreateSquare(startBox.transform.position,finishBox.transform.position);
                    currentSquare.GetComponent<Square>().setPosition(startBox.transform.position, finishBox.transform.position);
                    startBox.GetComponent<SpriteRenderer>().color = Color.white;
                    startBox = null;
                    finishBox = null;

                }
                

            }
        }
       
       
    }
    public void CreateSquare(Vector2 startLine,Vector2 finishLine)
    {
        
        float angleToRotate;
        float dist = Vector3.Distance(startLine, finishLine);
        Debug.Log((startLine, finishLine, dist));
        Vector2 positionSprite = finishLine - startLine;

        currentSquarePos = startLine + (positionSprite / 2);
        
        Vector2 dir = finishLine - startLine;

        if(finishLine.x>startLine.x)

            angleToRotate = Mathf.Atan2(dir.y, dir.x) * (180 / Mathf.PI);
        else
            angleToRotate =-1* Mathf.Atan2(dir.x,dir.y)  * (180 / Mathf.PI);


        currentSquare = Instantiate(squarePrefab, currentSquarePos, Quaternion.Euler(0, 0, angleToRotate));


        currentSquare.transform.localScale = positionSprite;
        if (positionSprite.x < positionSprite.y)
        {
            currentSquare.transform.localScale = new Vector2(0.2f, dist);
        }
        else if (positionSprite.x > positionSprite.y)
        {
            currentSquare.transform.localScale = new Vector2(dist, 0.2f);
        }

    }
    public void CreateLine(Vector2 trailStartPosition,Vector2 target)
    {
        GameObject currentTrail =Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        //currentTrail.GetComponent<CuttedLine>().setPosition(trailStartPosition,target);
    }

   
   





    /*void CreateLine()
    {

        fingerPosition.Clear();
        fingerPosition.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPosition.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, fingerPosition[0]);
        lineRenderer.SetPosition(1, fingerPosition[1]);
        edgeCollider.points = fingerPosition.ToArray();

    }
    //void UpdateLine(Vector2 newFingerPos)
    //{
    //    fingerPosition.Add(newFingerPos);
    //    lineRenderer.positionCount++;
    //    lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    //}
    */
}

