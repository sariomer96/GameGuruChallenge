using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public float inputSize;
    public float gridSize;
    public float padding;
    public Grid gridObject;
    public int count = 5;
    public int[,] gridArray;
    public int a, b;

    private Camera _camera;
    private Vector2 screenBounds;
    public    List<Grid> killList = new List<Grid>();
    public List<Grid> elementList = new List<Grid>();
    private float tmpPosX, tmpPosY;

    void SetGridSize(float rangeX,float rangeY)
    {
        float range;
        if (rangeX >= rangeY)
        {
            range = rangeY;
            
        }
        else
            range = rangeX;

        gridSize = (range / ((2 * count) + (count + 1)))*2;
        padding = gridSize/2 ;
        
        gridObject.transform.localScale =new Vector3( gridSize,gridSize,gridSize);
    }

   Vector2 GetStartCoordinate(float rangeX,float rangeY)
    {
        float range;
        if (rangeX >= rangeY)
        {
            range = rangeY;
            
        }
        else
            range = rangeX;

        float halfRange = range / 2;
        float startPosX = -halfRange + padding*2;             // 
        float startPosY = halfRange - padding * 2;
        
        
        return new Vector2(startPosX,startPosY);
    }
    
    // Start is called before the axis frame update
    void Start()
    {
        
    
      _camera=Camera.main;
     
      
        gridArray = new int[count, count];
         
        
        screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
       

        SpriteRenderer renderer= gridObject.transform.GetComponentInChildren<SpriteRenderer>();


     
      //  Instantiate(gridObject, new Vector3(screenBounds.x-(renderer.bounds.size.x/2),screenBounds.y-(renderer.bounds.size.y/2)), quaternion.identity); 
        
       
        float rangeX = Mathf.Abs(screenBounds.x*2);
        float rangeY = Mathf.Abs(screenBounds.y * 2)-inputSize;
      
        
        SetGridSize(rangeX,rangeY);






        Vector2 startPos;
          startPos = GetStartCoordinate(rangeX, rangeY);
       
        tmpPosX = startPos.x;
       
 
        tmpPosY = startPos.y;
       
     
     
     

      
        
       //  Camera.main.transform.position = new Vector3(grids.transform.position.x,grids.transform.position.y,Camera.main.transform.position.z);
        for (int i = 0; i < count; i++)
        {   
            for (int j = 0; j < count; j++)
            {
            
                gridArray[i, j] = 0;
                
               Grid grid= Instantiate(gridObject, new Vector2(tmpPosX, tmpPosY), quaternion.identity);
   
               elementList.Add(grid);
               grid.matrix.x = i;
               grid.matrix.y = j;

               tmpPosX += gridSize+padding;
               // newPosX += range;
              
            }

            tmpPosY -= gridSize+padding;
            tmpPosX = startPos.x;
            /*newPosX = startPosX;
            newPosY -= range;*/
            /*posX = -2;
            posY-=2;*/

        }
   


    }

   Grid GetGridIndexes()
    {
        RaycastHit2D hit;
         hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,LayerMask.GetMask("Grid"));

      
             
        if(hit.collider != null)
        {
           
        
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

    private bool IsItCorner(int matrixElement,int neighborIndex)
    {
        if (matrixElement+neighborIndex< 0  || matrixElement+neighborIndex>=count)
        {
            return true;
        }

        return false;
    }

    private List<Grid> GetNeighbors(Grid selectedGrid)
    {
        List<Grid> neighborsList = new List<Grid>();
        List<Grid> gridActive=GetActiveList();
         neighborsList.Clear();
     
        int x =(int) selectedGrid.matrix.x;
        int y = (int)selectedGrid.matrix.y;

        int[] neighborIndexList = { -1, 1 };


        for (int neighIndex = 0; neighIndex < neighborIndexList.Length; neighIndex++)
        {

            if (!IsItCorner(x,neighborIndexList[neighIndex])&&  gridArray[x+neighborIndexList[neighIndex],y] == 1)
            {
                Grid grid= gridActive.Where(a => a.matrix == new Vector2(x+neighborIndexList[neighIndex],y)).Select(a => a).First();
        
                neighborsList.Add(grid);
            }

            if (!IsItCorner(y,neighborIndexList[neighIndex])&&  gridArray[x,y+neighborIndexList[neighIndex]] == 1)
            {
                Grid grid= gridActive.Where(a => a.matrix == new Vector2(x,y+neighborIndexList[neighIndex])).Select(a => a).First();
        
                neighborsList.Add(grid);
            }
        }
    
   

        return neighborsList;
    }



}
