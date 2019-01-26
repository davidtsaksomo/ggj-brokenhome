using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyController : MonoBehaviour
{
    public GameObject[] images;
    public Transform player;
    public float cycleSteps;
    int currentIndex = 0;
    int currentStep = 1;
    Color currentColor;
    void Update()
    {
        if(player.position.x > currentStep * cycleSteps)
        {
            ChangeSkyIndex();
            currentStep += 1;
            player.localScale = new Vector3(player.localScale.x, player.localScale.y + 0.02f, player.localScale.z);
        }
    }

    void ChangeSkyIndex()
    {
        foreach(GameObject image in images)
        {
            image.SetActive(false);
        }
        int newIndex = (currentIndex + 1) % images.Length;
        if(newIndex != 0)
            images[currentIndex].SetActive(true);
        images[newIndex].SetActive(true);
        currentColor = images[newIndex].GetComponent<Image>().color;
        images[newIndex].GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        StartCoroutine(FadeIn(newIndex));

    }

    IEnumerator FadeIn(int newIndex)
    {
        float alpha = 0;
        while(alpha < 1f)
        {
            alpha += 0.025f;
            images[newIndex].GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        images[currentIndex].SetActive(false);
        currentIndex = newIndex;
    }
}
