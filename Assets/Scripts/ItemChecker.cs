using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public TextMesh ending;
    public DialogEvent dialogEvent;
    public GameObject blocker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void  OnTriggerEnter2D ( Collider2D item){
		if(item.tag == "itemchecker"){
            if(checkitems()==0){
                Destroy(blocker);
            }
            else{
                ending.text = "You missed "+checkitems()+" items, restart?";
            }
		}
	}
	void  OnTriggerExit2D ( Collider2D item){
		if(item.tag == "itemchecker"){
            ending.text = "";
		}
	}
    public int checkitems(){
        int counted = 0;
        for (int i = 0; i < 5; i++)
        {
            if(GameController.gameController.gameObject.GetComponent<ItemController>().getItem(i)==true){
                counted++;
            }
        }
        return 4-counted;
    }
}
