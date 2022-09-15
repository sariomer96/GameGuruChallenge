using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int count = 5;
    public int[,] gridArray;
    public int a, b;
         
    // Start is called before the first frame update
    void Start()
    {
        gridArray = new int[count, count];
 
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                gridArray[i, j] = 0;
            }
       
        }
        
    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gridArray[a, b] = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    print("VALUE: "+gridArray[i, j]+"   "+i+" "+j);
                }
       
            }
            
        }
    }
}
