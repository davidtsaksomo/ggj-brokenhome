using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnightController : MonoBehaviour
{
    public GameObject knight;
    public Sprite attackSprite;
    public Sprite defendSprite;
    private bool shield = false;
    void Update()
    {
        if (Input.GetButtonDown("Knight"))
        {
            knight.SetActive(true);
        }
        if (Input.GetButtonUp("Knight"))
        {
            knight.SetActive(false);
        }
        if (Input.GetAxis("Shield") == 1 && !shield)
        {
            knight.GetComponent<SpriteRenderer>().sprite = defendSprite;
            shield = true;
        }
        if (Input.GetAxis("Shield") == 0 && shield)
        {
            knight.GetComponent<SpriteRenderer>().sprite = attackSprite;
            shield = false;
        }
    }
}
