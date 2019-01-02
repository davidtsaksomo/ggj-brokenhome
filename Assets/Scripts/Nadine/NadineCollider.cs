using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadineCollider : MonoBehaviour
{
    public PlayerController player;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy" || (coll.gameObject.tag == "Harmful"))
        {
            player.Respawn();
        }
    }
}
