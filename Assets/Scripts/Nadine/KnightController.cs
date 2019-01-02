using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnightController : MonoBehaviour
{
    public GameObject knight;
    public GameObject knightpivot;

    [HideInInspector]
    public bool shield = false;
    [HideInInspector]
    public bool active = false;

    void Update()
    {
        //Activating the knight
        if (!active && (Input.GetAxis("Knight") == 1 || Input.GetAxis("Shield") == 1))
        {
            knight.SetActive(true);
            knight.GetComponent<Animator>().SetBool("shield", shield);
            active = true;
        }
        if (active && (Input.GetAxis("Knight") == 0 && Input.GetAxis("Shield") == 0))
        {
            knight.SetActive(false);
            active = false;
        }

        //Toggle between attack and defense
        if (!shield && Input.GetAxis("Shield") == 1)
        {
            knight.GetComponent<Animator>().SetBool("shield", true);
            shield = true;
        }
        if (shield && Input.GetAxis("Knight") == 1 && Input.GetAxis("Shield") == 0)
        {
            knight.GetComponent<Animator>().SetBool("shield", false);
            shield = false;
        }
        //Rotating the knight
        float rotation = Input.GetAxis("RightVertical");
        knightpivot.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rotation * 20f);
    }
}
