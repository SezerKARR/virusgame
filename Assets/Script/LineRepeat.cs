using UnityEngine;

public class LineRepeat : MonoBehaviour
{
    public Line line;
    public GameObject linePrefab;
    GameObject currentLine;
    private GameObject startBox;
    private GameObject finishBox;
    public GameObject cuttedLinePrefab;
    private Color boxColor;
    public GameObject liner;
    GameObject line1;
    public GameObject targetCircle;
    GameObject[] circler = new GameObject[80];
    LineRenderer lineRendererLiner;
    int maxI = 0;
    

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


                if (hit.collider.tag == "Player"|| hit.collider.tag == "EnemyRed")
                {
                    if (startBox == null)
                    {
                        
                        startBox = hit.transform.gameObject;
                        boxColor = startBox.GetComponent<SpriteRenderer>().color;
                        startBox.GetComponent<SpriteRenderer>().color = Color.cyan;

                        line1 = Instantiate(liner, Vector3.zero, Quaternion.identity);

                    }
                    
                    else if (startBox == hit.collider.gameObject)
                    {
                        startBox.GetComponent<SpriteRenderer>().color = boxColor;
                        startBox = null;
                        finishBox = null;
                    }




                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (line1 != null)
            {


                lineRendererLiner = line1.GetComponent<LineRenderer>();

                Vector2 worldPoint1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRendererLiner.SetPosition(0, startBox.transform.position);
                
                
                int myDistances = (int)Vector2.Distance(lineRendererLiner.GetPosition(0), worldPoint1);
                lineRendererLiner.SetPosition(1, worldPoint1);
                lineRendererLiner.SetColors(Color.green, Color.green);
                if (startBox.GetComponent<Box>().GetCanLineFloat() < myDistances*2)
                {
                    lineRendererLiner.SetColors(Color.red, Color.red);

                }
                if (myDistances > 0)
                {
                    Vector3[] distancesResult = new Vector3[myDistances];
                    posToChunkDistances(lineRendererLiner.GetPosition(0), worldPoint1, distancesResult, myDistances, true, 1);

                    for (int i = 0; i < distancesResult.Length; i++)
                    {

                        Destroy(circler[i]);
                        circler[i] = Instantiate(targetCircle, distancesResult[i], Quaternion.identity);
                        circler[i].transform.SetParent(line1.transform);
                        if (maxI <= i)
                        {
                            maxI = i;
                        }

                        if (startBox.GetComponent<Box>().GetCanLineFloat() < Vector2.Distance(lineRendererLiner.GetPosition(0)
                            , circler[i].transform.position)*2)
                        {
                            circler[i].GetComponent<SpriteRenderer>().color = Color.red;

                        }


                    }

                    for (int x = distancesResult.Length; x < maxI + 1; x++)
                    {

                        Destroy(circler[x]);
                    }


                }

            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if ((hit.collider.tag == "Player" || hit.collider.tag == "EnemyRed" || hit.collider.tag == "EnemyYellow" || hit.collider.tag == "Unside") && startBox != null)
                {

                    if (hit.collider.gameObject != startBox)
                    {

                        finishBox = hit.transform.gameObject;
                        startBox.GetComponent<SpriteRenderer>().color = boxColor;

                        CreateLine(startBox, finishBox);
                        startBox = null;
                        finishBox = null;
                    }
                    


                }

            }
            
            Destroy(line1);
        }
        
        if (startBox != null&&!startBox.transform.gameObject.GetComponent<Box>().GetCanLine())
        {

            startBox.GetComponent<SpriteRenderer>().color = Color.green;
            Destroy(line1);
            startBox = null;
            finishBox = null;
        }
    }
    public void CreateLine(GameObject startBox,GameObject finishBox)
    {
        if (startBox.GetComponent<Box>().getLinesPosition(finishBox.transform.position) == -1)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            currentLine.GetComponent<Line>().SetPosition(startBox, finishBox);
            startBox.GetComponent<Box>().SetLine(currentLine);
            

        }


    }
    public void CreateCuttedLine(Vector2 startPosition, GameObject target)
    {

        GameObject currentTrail = Instantiate(cuttedLinePrefab, Vector3.zero, Quaternion.identity);
        currentTrail.GetComponent<CuttedLine>().setPosition(startPosition, target);
    }
   
    void posToChunkDistances(Vector3 from, Vector3 to, Vector3[] result, int chunkAmount, bool useOffset = false, float offsetDistance = -1f)
    {

        //Change from and to values if we want to use offset
        if (useOffset)
        {

            //Un-Comment to Allow negative offset?
            //offsetDistance = Mathf.Abs(offsetDistance);

            //Find new 'From' Distance
            Ray offsetRayFrom = new Ray();
            offsetRayFrom.origin = from;
            offsetRayFrom.direction = to - from;
            Vector3 newFromDisance = offsetRayFrom.GetPoint(offsetDistance);
            from = newFromDisance; //Change 'From' Distance


            //Find new 'To' Distance
            Ray offsetRayTo = new Ray();
            offsetRayTo.origin = to;
            offsetRayTo.direction = from - to;
            Vector3 newToDisance = offsetRayTo.GetPoint(offsetDistance);
            to = newToDisance; //Change 'To' Distance
        }

        //divider must be between 0 and 1
        float divider = 1f / chunkAmount;
        float linear = 0f;

        if (chunkAmount == 0)
        {
            Debug.LogError("chunkAmount Distance must be > 0 instead of " + chunkAmount);
            return;
        }

        if (chunkAmount == 1)
        {
            result[0] = Vector3.Lerp(from, to, 0.5f); //Return half/middle point
            return;
        }

        for (int i = 0; i < chunkAmount; i++)
        {
            if (i == 0)
            {
                linear = divider / 2;
            }
            else
            {
                linear += divider; //Add the divider to it to get the next distance
            }
            // Debug.Log("Loop " + i + ", is " + linear);
            result[i] = Vector3.Lerp(from, to, linear);
        }
    }


}

