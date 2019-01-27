using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    bool once = true;

    public AudioClip ParentFightSFX;
    public AudioSource source;

    void Awake(){
        source = GetComponent<AudioSource>();
    }
    //void Update()
    //{
        
    //}
    void OnTriggerEnter2D(Collider2D coll)
    {
        float vol = 50;
        if (once){
            source.PlayOneShot(ParentFightSFX,vol);
            once = false;
        }
    }
}

