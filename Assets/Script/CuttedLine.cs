using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttedLine : MonoBehaviour
{
    public LineRepeat lineRepeat;
    private GameObject gameController;
    public List<Vector2> boxPosition;
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public Vector3[] Positions = new Vector3[2];
    public float speed=2f;
    private Box box;
    void Awake()
    {

        gameController = GameObject.FindGameObjectWithTag("GameController");
        lineRepeat = gameController.GetComponent<LineRepeat>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        

    }
    public void setPosition(Vector2 startLine, GameObject finishLine)
    {
        lineRenderer.SetPosition(0, startLine);
        lineRenderer.SetPosition(1, finishLine.transform.position);
        box = finishLine.GetComponent<Box>();
        
    }
    private void FixedUpdate()
    {

        
        Vector3 chase = Vector3.MoveTowards(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), .1f); 
        lineRenderer.SetPosition(0, chase);
        if (lineRenderer.GetPosition(0) == lineRenderer.GetPosition(1))
        {
            Destroy(gameObject);
        }
        

    }

    void OnMouseExit()
    {
      
    }
}
