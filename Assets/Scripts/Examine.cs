using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    Camera mainCam;//Camera Object Will Be Placed In Front Of
    GameObject clickedObject;//Currently Clicked Object
    //Holds Original Postion And Rotation So The Object Can Be Replaced Correctly
    Vector3 originaPosition;
    Vector3 originalRotation;
 
    //If True Allow Rotation Of Object
    bool examineMode;
    bool isTaking = false;

    int itemNum = 0;
 
    void Start()
    {
        mainCam = Camera.main;
        examineMode = false;
    }
 
    private void Update()
    {    
        ClickObject();//Decide What Object To Examine
                 
        TurnObject();//Allows Object To Be Rotated
                 
        ExitExamineMode();//Returns Object To Original Postion
    }
 
 
   void ClickObject()
    {
        if (Input.GetMouseButtonDown(0) && isTaking)
        {
            if(mainCam==null){
                Debug.Log("NoObject");
            }
            else{
                Debug.Log("ClickedObject");
            clickedObject.transform.position = mainCam.transform.position + (transform.forward * 3f);
            examineMode = true;
            Time.timeScale = 0;
            }
            itemNum = checkName(clickedObject.name);
            if(itemNum == 0){
                Debug.Log("item name not found");
            }
            else{
                Debug.Log(itemNum);
                if(GameController.gameController.gameObject.GetComponent<ItemController>()==null){
                    Debug.Log("null");
                }
                else{
                GameController.gameController.gameObject.GetComponent<ItemController>().addItem(itemNum);
                Debug.Log(GameController.gameController.gameObject.GetComponent<ItemController>().getItem(1));  
                }
            }
        }
                     
    }  
 
    void TurnObject()
    {
        if (Input.GetMouseButton(0) && examineMode)
        {
            float rotationSpeed = 15;
 
            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;
 
            clickedObject.transform.Rotate(Vector3.up, -xAxis, Space.World);         
            clickedObject.transform.Rotate(Vector3.right, yAxis, Space.World);
        }
    }
 
    void ExitExamineMode()
    {       
       if (Input.GetMouseButtonDown(1) && examineMode)
       {               
           //Reset Object To Original Position
           clickedObject.transform.position = originaPosition;
           clickedObject.transform.eulerAngles = originalRotation;
 
           //Unpause Game
           Time.timeScale = 1;
 
           //Return To Normal State
           examineMode = false;

           Destroy(clickedObject);
       }      
    }


    void  OnTriggerEnter2D ( Collider2D item){
		if(item.tag == "Grabable"){
			clickedObject = item.gameObject;
            originaPosition = clickedObject.transform.position;
            originalRotation = clickedObject.transform.rotation.eulerAngles;
			isTaking = true;
		}
	}
	void  OnTriggerExit2D ( Collider2D item){
		if(item.tag == "Grabable"){
			isTaking = false;
		}
	}

    int checkName(string i)
    {
        if(i == "item1"){
            return 1;
        }
        if(i == "item2"){
            return 2;
        }
        if(i == "item3"){
            return 3;
        }
        if(i == "item4"){
            return 4;
        }
        return 0;
    }
}
