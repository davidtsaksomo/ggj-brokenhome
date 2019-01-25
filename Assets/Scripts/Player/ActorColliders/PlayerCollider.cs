using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Actor actor;

    void OnTriggerEnter2D(Collider2D coll)
    {
        print(coll.tag);
    }
}
