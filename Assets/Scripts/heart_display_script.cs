using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heart_display_script : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public float distance;
    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i<maxHealth; i++)
        {
            GameObject imgObject = new GameObject("Heart_"+i);
            hearts.Add(imgObject);
            imgObject.transform.SetParent(gameObject.transform.parent, false);
            Image newImage = imgObject.AddComponent<Image>();
            newImage.sprite = fullHeart;
        }
        UpdateHearts();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateHearts()
    {
        for (int i = 0; i<currentHealth; i++)
        {
            hearts[i].GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
            hearts[i].GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x + i*distance, GetComponent<RectTransform>().position.y , 0);
            hearts[i].GetComponent<Image>().sprite = fullHeart;
        }
        for (int i = 0; i<maxHealth-currentHealth; i++)
        {
            hearts[i+currentHealth].GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
            hearts[i+currentHealth].GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x + (i+currentHealth)*distance, GetComponent<RectTransform>().position.y , 0);
            hearts[i+currentHealth].GetComponent<Image>().sprite = emptyHeart;
        }
    }
}
