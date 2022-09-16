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
 
    
       public    List<Grid> killList = new List<Grid>();
    public List<Grid> elementList = new List<Grid>();
 
 
         
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

 void ActivateGrid(Grid grid)
 {
    
    gridArray[(int)grid.matrix.x,(int)grid.matrix.y]=1;
    grid.transform.GetChild(0).transform.gameObject.SetActive(true);

 }

 Vector2 GetMatrixIndexes(int index)
 {
      int row = index/count;
      int column=index-(row*count);

     return new Vector2(row,column);
 }
 List<Grid> GetActiveList()
  {
    List<Grid> activeList=new List<Grid>();
      for(int i=0; i<elementList.Count; i++)
      {
           Vector2 matrix= GetMatrixIndexes(i);
             if(gridArray[(int)matrix.x,(int)matrix.y] ==1 )
             {
               activeList.Add(elementList[i]);
             }
      }
      return activeList;

  }
  
  void RemoveItems()
  {
         foreach(var item in killList)
         {
              DeactivateGrid(item);

         }
         killList.Clear();
  }
  void DeactivateGrid(Grid grid)
  {
          gridArray[(int)grid.matrix.x,(int)grid.matrix.y]=0;
          grid.transform.GetChild(0).transform.gameObject.SetActive(false);

  }
  void AddKillList()
  {

    List<Grid> grids =GetActiveList();

    foreach(var item in grids)
    {
      List<Grid> neighbors  =  GetNeighbors(item);
             if(neighbors.Count>=2)
             {
                killList.AddRange(neighbors);
                killList.Add(item);
             }
               
    }


  }
    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            Grid grid = GetGridIndexes();

           if (grid==null)  // !=null
              return;
               
           ActivateGrid(grid);
           
          AddKillList();
          RemoveItems();

        }

        
    }

    private List<Grid> GetNeighbors(Grid selectedGrid)
    {
        List<Grid> neighborsList = new List<Grid>();
        List<Grid> gridActive=GetActiveList();
         neighborsList.Clear();
     
        int x =(int) selectedGrid.matrix.x;
        int y = (int)selectedGrid.matrix.y;
   
    

    
        // x axis
        if (x+1 < count && gridArray[x+1,y] == 1)
        {
            // print(x+","+(y+1)+count);
        
            Grid grid= gridActive.Where(a => a.matrix == new Vector2(x+1,y)).Select(a => a).First();
        
            neighborsList.Add(grid);
        }

        if (x-1 >= 0 && gridArray[x-1,y] == 1)
        {
            // print(x+","+(y+1)+count);
        
            Grid grid= gridActive.Where(a => a.matrix == new Vector2(x-1,y)).Select(a => a).First();
        
            neighborsList.Add(grid);
        }


        // y axis
        if (y+1 < count && gridArray[x,y+1] == 1)
        {
            // print(x+","+(y+1)+count);
        
            Grid grid= gridActive.Where(a => a.matrix == new Vector2(x,y+1)).Select(a => a).First();
        
            neighborsList.Add(grid);
        }

        if (y-1 >= 0 && gridArray[x,y-1] == 1)
        {
            // print(x+","+(y+1)+count);
        
            Grid grid= gridActive.Where(a => a.matrix == new Vector2(x,y-1)).Select(a => a).First();
        
            neighborsList.Add(grid);
        }

        return neighborsList;
    }



}
