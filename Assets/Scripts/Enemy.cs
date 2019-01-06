using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health;
    
    public void TakeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(Blink());
        if (health <= 0)
            Destroy(this.gameObject);
    }

    public IEnumerator Blink()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return null;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
