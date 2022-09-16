using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Grid gridObject;
    public int count = 5;
    public int[,] gridArray;
    public int a, b;
   List<List<int>> activeList = new List<List<int>>();
         
    // Start is called before the first frame update
    void Start()
    {
        gridArray = new int[count, count];
        float posX = -2;
        float posY = 3;
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                gridArray[i, j] = 0;
               Grid grid= Instantiate(gridObject, new Vector2(posX, posY), quaternion.identity);
               grid.matrix.x = i;
               grid.matrix.y = j;
           
                posX+=2;
            }

            posX = -2;
            posY-=2;

        }
        
    
    }

    Vector2  GetGridIndexes()
    {
        RaycastHit2D hit;
         hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,LayerMask.GetMask("Grid"));

      
             
        if(hit.collider != null)
        {
           
            Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position+" "+ hit.collider.gameObject.name);
            Grid grid = hit.transform.GetComponentInChildren<Grid>();
            grid.transform.GetChild(0).gameObject.SetActive(true);
            return grid.matrix;

        }

        return Vector2.negativeInfinity;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           Vector2 matrix= GetGridIndexes();

           if (matrix!=Vector2.negativeInfinity)
           {
               
               GetNeighbors((int)matrix.x,(int)matrix.y);
           }
           
           
           
            gridArray[a, b] = 1;
            var list=new List<int> {a,b};
         
             bool isContain=false;
           for(int i=0; i<activeList.Count; i++)
           {
                
                if(activeList[i].Contains(a)&&activeList[i].Contains(b))
                {
                   isContain=true;
                }

           }
           if(isContain==false)
             activeList.Add(list);
          
           

          //    print(activeList.Count);
            //print(activeDict.count);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                  //  print("VALUE: "+gridArray[i, j]+"   "+i+" "+j);
                }
       
            }
            
        }
        if(Input.GetMouseButtonDown(1))
        {
            GetNeighbors(a,b);
        }
    }

    private void GetNeighbors(int row,int column)
    {
        int x = row;

        for (int i = 0; i < 2; i++)
        {
            if (x+1<count)  
            {
                if(i==0)
                {
                    print((x+1)+","+column);
                   // (x+1,b)
                }else{
                    print(row+","+(x+1));
                    //(a,x+1)
                }
              
            }

            if (x-1>-1)
            {
                   if(i==0)
                {
                    print((x-1)+","+column);
                }else{
                   print(row+","+(x-1));
                }
               
            }

            x = column;
        }
    }
}
