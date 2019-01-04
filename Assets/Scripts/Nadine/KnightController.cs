using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnightController : MonoBehaviour
{
    public GameObject knight;

    [HideInInspector]
    public bool shield = false;
    [HideInInspector]
    public bool active = false;
    private bool somersault = false;
    void Update()
    {


        if (somersault)
        {
            if (!knight.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("knight_somersault"))
            {
                somersault = false;
            }
        }
        else
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
                knight.layer = LayerMask.NameToLayer("Shield");
                shield = true;
            }
            if (shield && Input.GetAxis("Knight") == 1 && Input.GetAxis("Shield") == 0)
            {
                knight.GetComponent<Animator>().SetBool("shield", false);
                knight.layer = LayerMask.NameToLayer("Lance");
                shield = false;
            }
            if (Input.GetButtonDown("Somersault"))
            {
                knight.SetActive(true);
                active = true;
                somersault = true;
                knight.GetComponent<Animator>().SetTrigger("somersault");

            }
        }


    }

    public void Rotate(float rotation)
    {
        if(active && !somersault)
        {
            knight.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }
}
