using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public List<GameObject> allBox;
    public List<GameObject> enemyBox ;
    LineRepeat lineRepeat;
    int enemyBoxs;
    public Image finishText;
    // Start is called before the first frame update
    public void Start()
    {
       
        enemyBox.Clear();
        for (int i = 0; i < allBox.Count; i++)
        {
            if (allBox[i].CompareTag("EnemyYellow") || allBox[i].CompareTag("EnemyRed"))
            {
                enemyBox.Add(allBox[i]);

            }


        }

        lineRepeat = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineRepeat>();
        if (enemyBox.Count == 0)
        {

            finishText.gameObject.SetActive(true);
        }
    }
    

    public void Movement()
    {


        int deger = Random.Range(0, allBox.Count);

        float dist = Vector2.Distance(enemyBox[enemyBoxs].transform.position
             , allBox[deger].transform.position);
        if (enemyBox[enemyBoxs] != allBox[deger] && dist*2 <= enemyBox[enemyBoxs].GetComponent<Box>().GetCanLineFloat())
        {

            lineRepeat.CreateLine(enemyBox[enemyBoxs], allBox[deger]);
            enemyBoxs++;
            if (enemyBoxs == enemyBox.Count)
            {
                enemyBoxs = 0;
            }
        }

    }
}
