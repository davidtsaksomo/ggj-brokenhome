using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorCollider2D : MonoBehaviour
{
    public Actor actor;

    void Awake()
    {
        CapsuleCollider2D collider = gameObject.AddComponent(typeof(CapsuleCollider2D)) as CapsuleCollider2D;
        actor = GetComponent<Actor>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy" || (coll.gameObject.tag == "Harmful"))
        {
            actor.Respawn();
        }
    }
}
