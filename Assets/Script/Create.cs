using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    public Transform[] points;
    public GameObject GameObj;
    public float GameObjectAmount = 2;
    void Start()
    {
        duplicateObject(GameObj, (int)GameObjectAmount);
    }
    public void duplicateObject(GameObject original, int howmany)
    {
        howmany++;
        for (int i = 0; i < points.Length - 1; i++)
        {
            for (int j = 1; j < 18; j++)
            {
                Vector3 position = points[i].position + j * ((points[i + 1].position - points[i].position) / 18);
                Instantiate(original, position, Quaternion.identity);
            }
        }
    }

}