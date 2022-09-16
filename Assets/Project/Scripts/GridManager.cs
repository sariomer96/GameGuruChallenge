using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Grid gridObject;
    public int count = 5;
    public int[,] gridArray;
    public int a, b;
    public List<Grid> neighborsList = new List<Grid>();
    public List<Grid> killList = new List<Grid>();
    public List<Grid> elementList = new List<Grid>();
 //   public  List<>
  // List<List<int>> activeList = new List<List<int>>();
   List<Grid> activeList = new List<Grid>();
         
    // Start is called before the axis frame update
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
               elementList.Add(grid);
               grid.matrix.x = i;
               grid.matrix.y = j;
           
                posX+=2;
            }

            posX = -2;
            posY-=2;

        }
        
    
    }

   Grid GetGridIndexes()
    {
        RaycastHit2D hit;
         hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,LayerMask.GetMask("Grid"));

      
             
        if(hit.collider != null)
        {
           
            Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position+" "+ hit.collider.gameObject.name);
            Grid grid = hit.transform.GetComponentInChildren<Grid>();
            grid.transform.GetChild(0).gameObject.SetActive(true);
            return grid;

        }

        return null;
    }

    private void Update()
    {
        print(activeList.Count);
        if (Input.GetMouseButtonDown(0))
        {
            Grid grid = GetGridIndexes();

           if (grid!=null)  // !=null
           {
               
             //  List<Vector2> neighbors= GetNeighbors(grid.matrix);
             gridArray[(int)grid.matrix.x,(int) grid.matrix.y] = 1;

           }
           
           
           
           // gridArray[a, b] = 1;
          
         
             bool isContain=false;
           for(int i=0; i<activeList.Count; i++)
           {
               if (activeList[i].matrix==new Vector2((int)grid.matrix.x,(int)grid.matrix.y))
               {
                   isContain=true;
               }
                /*if(activeList[i].Contains(a)&&activeList[i].Contains(b))
                {
                   isContain=true;
                }*/

           }
           if(isContain==false)
             activeList.Add(grid);




           for (int i = 0; i < activeList.Count; i++)
           {
              List<Grid> neighbors=  GetNeighbors(activeList[i]);

              if (neighbors.Count>=2)
              {
                  killList.AddRange(neighbors);
                  killList.Add(activeList[i]);
              }
              
              
              
           }

           foreach (var VARIABLE in killList)
           {
               VARIABLE.transform.GetChild(0).gameObject.SetActive(false);
               
           }
           

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
        
    }

    private List<Grid> GetNeighbors(Grid selectedGrid)
    {
        neighborsList.Clear();
     
        int x =(int) selectedGrid.matrix.x;
        int y = (int)selectedGrid.matrix.y;
        int axis= x;
        for (int i = 0; i < 2; i++)
        {
            if (axis+1<count)  
            {
                if(i==0)
                {
                  
                    if (gridArray[x+ 1,y] == 1)
                    {
                        
                      Grid grid= activeList.Where(a => a.matrix == new Vector2(x+1, y)).Select(a => a).First();
                 
                      
                      neighborsList.Add(grid);
                    }
              
                    // (x+1,b)
                }
                
                else{
                    if (gridArray[x,y+1] == 1)
                    {
                        print(x+","+(y+1)+count);
                   
                        Grid grid= activeList.Where(a => a.matrix == new Vector2(x,y+1)).Select(a => a).First();
                 
                        neighborsList.Add(grid);
                    }
                       
                 
                 
                    //(a,x+1)
                }
              
            }

            if (axis-1>-1)
            {
                   if(i==0) 
                   {
                
                    if (gridArray[x - 1, y] == 1)
                    {
                        print((x-1)+","+y);
                        Grid grid= activeList.Where(a => a.matrix == new Vector2(x-1,y)).Select(a => a).First();
                 
                        neighborsList.Add(grid);
                    }
                        
                    
                     }
                   else
                   {
                       if (gridArray[x, y - 1] == 1)
                       {
                           Grid grid= activeList.Where(a => a.matrix == new Vector2(x,y-1)).Select(a => a).First();
                 
                           neighborsList.Add(grid);
                       }
                     
                       
                   print(x+","+(x-1));
                   }
               
            }

            axis = y;

        }

        return neighborsList;
    }
}
