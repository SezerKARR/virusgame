using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public SpriteRepeat spriteRepeat;
    private GameObject gameController;
    public List<Vector2> boxPosition;
    private Vector3[] positionBox = new Vector3[2];
    public 
    void Awake()
    {

        gameController = GameObject.FindGameObjectWithTag("GameController");
        spriteRepeat = gameController.GetComponent<SpriteRepeat>();
        
       


    }
    public void setPosition(Vector3 startLine, Vector3 finishLine)
    {
       positionBox[0]=startLine;
        positionBox[1] = finishLine;
       
        
    }
    void OnMouseDown()
    {
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        
        spriteRepeat.CreateLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), positionBox[0]);
        spriteRepeat.CreateLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), positionBox[1]);
        Destroy(gameObject);
    }
    
}
