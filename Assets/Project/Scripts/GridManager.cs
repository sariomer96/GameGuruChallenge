using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public float gridSize;
    public float padding; // space between boxes(stack)
    public int[,] gridArray; // holds if objects at screen is one or zero  
    public    List<Grid> killList = new List<Grid>();  // this variable holds the elements to be deleted
    public List<Grid> elementList = new List<Grid>();  // holds all grids 
    public float tmpPosX, tmpPosY;
 


    private void Awake()
    {
        instance = this;
    }

    float GetRangeForFitScreen( float rangeX,float rangeY)  // returns minimum range, we have two range horizontal and vertical 
    {
        float range;
       
        if (rangeX >= rangeY)
            range = rangeY;
        else
            range = rangeX;

        return range;

    }
  public  void SetGridSize(float rangeX,float rangeY)
    {
        float range=GetRangeForFitScreen(rangeX,rangeY);
        
        gridSize = (range / ((2 * GameManager.instance.count) + (GameManager.instance.count + 1)))*2;
        padding = gridSize/2 ;
        
        GameManager.instance.gridObject.transform.localScale =new Vector3( gridSize,gridSize,gridSize);
    }

   public Vector2 GetStartCoordinate(float rangeX,float rangeY) //  padding*2= grid scale
    {
        float range=GetRangeForFitScreen(rangeX,rangeY);

        float halfRange = range / 2;
        float startPosX = -halfRange + padding*2;             
        float startPosY = halfRange - padding * 2;
        
        return new Vector2(startPosX,startPosY);
    }
    
    // Start is called before the axis frame update
  
    public void CreateMatrix(float startPos_x)
    {
        for (int i = 0; i < GameManager.instance.count; i++)
        {   
            for (int j = 0; j < GameManager.instance.count; j++)
            {
                gridArray[i, j] = 0;
                
                Grid grid= Instantiate(GameManager.instance.gridObject, new Vector2(tmpPosX, tmpPosY), quaternion.identity);
   
                elementList.Add(grid);
                grid.matrix.x = i;
                grid.matrix.y = j;

                tmpPosX += gridSize+padding;    // padding*2=grid size

            }

            tmpPosY -= gridSize+padding;
            tmpPosX =startPos_x;

        }
    }
         
    private Grid GetGrid()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,LayerMask.GetMask("Grid"));

        if(hit.collider != null)
        {
            Grid grid = hit.transform.GetComponentInChildren<Grid>();
            return grid;
        }
        return null;
    }

 private void ActivateGrid(Grid grid)
 {
    gridArray[(int)grid.matrix.x,(int)grid.matrix.y]=1;
    grid.transform.GetChild(0).transform.gameObject.SetActive(true);
 }

 private Vector2 GetMatrixIndexes(int index)  // returns matrix indexes of active item in gridArray . Active list is a list . gridArray is a matrix
 {
      int row = index/GameManager.instance.count;
      int column=index-(row*GameManager.instance.count);

     return new Vector2(row,column);
 }
 private List<Grid> GetActiveList()
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
  
  private void RemoveItems()
  {
         foreach(var item in killList)
             DeactivateGrid(item);

         killList.Clear();
  }
  void DeactivateGrid(Grid grid)
  {
      gridArray[(int)grid.matrix.x,(int)grid.matrix.y]=0;
      grid.transform.GetChild(0).transform.gameObject.SetActive(false);

  }
  void CreateKillList()   
  {

    List<Grid> grids =GetActiveList();

    foreach(var item in grids)
    {
      List<Grid> neighbors  =  GetActiveNeighbors(item);
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
            Grid grid = GetGrid();
       
           if (grid==null)  
              return;
           ActivateGrid(grid);
           
          CreateKillList();
          RemoveItems();
        }

    }

    private bool IsItCorner(int matrixElement,int neighborIndex)
    {
        if (matrixElement+neighborIndex< 0  || matrixElement+neighborIndex>=GameManager.instance.count)
          return true;
    
        return false;
    }

    private List<Grid> GetActiveNeighbors(Grid selectedGrid)
    {
        List<Grid> neighborsList = new List<Grid>();
        List<Grid> gridActive=GetActiveList(); 
        neighborsList.Clear();
     
        int x =(int) selectedGrid.matrix.x;
        int y = (int)selectedGrid.matrix.y;

        int[] neighborIndexList = { -1, 1 };

        for (int i = 0; i < neighborIndexList.Length; i++)
        {
            if (!IsItCorner(x,neighborIndexList[i]) &&  gridArray[x+neighborIndexList[i],y] == 1)
            {
                Grid  grid= gridActive.Where(a => a.matrix == new Vector2(x+neighborIndexList[i],y)).Select(a => a).First();
                neighborsList.Add(grid);
            }
            if (!IsItCorner(y,neighborIndexList[i]) &&  gridArray[x,y+neighborIndexList[i]] == 1)
            {
                Grid  grid= gridActive.Where(a => a.matrix == new Vector2(x,y+neighborIndexList[i])).Select(a => a).First();
                neighborsList.Add(grid);
            }
        }
        return neighborsList;
    }
}
