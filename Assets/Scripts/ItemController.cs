using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    bool[] items;
    // Start is called before the first frame update
    void Start()
    {
        items = new bool[5];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getItem(int i)
    {
        return items[i];
    }
    public void addItem(int i)
    {
        if(items[i]==false){
            items[i] = true;
        }
    }

    void refreshItem()
    {
        for (int i = 0; i < 5; i++)
        {
            items[i]=false;
        }
    }
}
