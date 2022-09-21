using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    
    public static GameManager instance;
    public Grid gridObject;
    public int count = 5;  // count*count matrix 
 
    private Camera _camera;
    private Vector2 screenBounds; // camera bounds 
    [SerializeField] TMP_InputField matrixInputField;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        count = PlayerPrefs.GetInt("count");
        _camera=Camera.main;
     
        screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
       
        SpriteRenderer renderer= gridObject.transform.GetComponentInChildren<SpriteRenderer>();
        
        CreateGrid();
    }

    public void CreateGrid()
    {
         
        count = int.Parse(matrixInputField.text);
        PlayerPrefs.SetInt("count",count);
        if (count<2)
            return;
        GridManager.instance.gridArray = new int[count, count];
        float rangeX = Mathf.Abs(screenBounds.x*2);   //horizontal range
        float rangeY = Mathf.Abs(screenBounds.y * 2);  // vertical range

        GridManager.instance.SetGridSize(rangeX,rangeY);

        Vector2 startPos; 
        startPos = GridManager.instance.GetStartCoordinate(rangeX, rangeY);
       
        GridManager.instance.tmpPosX = startPos.x;
        GridManager.instance.tmpPosY = startPos.y;
        GridManager.instance.CreateMatrix(startPos.x);
       
       
    }
    
    public void Rebuild()
    {
        int count = GridManager.instance.elementList.Count;

        for (int i = 0; i < count; i++)
        {
            Destroy(GridManager.instance.elementList[0].gameObject);
            GridManager.instance.elementList.RemoveAt(0);
            
        }
       
    }

 
    
}
