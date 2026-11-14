using System.Collections.Generic;
using UnityEngine;

public class set_map_script : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    [Header("Bounds")]
    public float yTOP = 72.5f;
    public float yBOTTOM = -72.5f;
    public float xRIGHT = 72.5f;
    public float xLEFT = 72.5f;
    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        SetMap();
    }
    public void SetMap()
    {
        SetPlayer();
        SetEnemies(3);
    }

    void SetPlayer()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            player.GetComponent<Rigidbody2D>().position = new Vector3(0, 0, 0);
        }
    }

    void SetEnemies(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject e = Instantiate(enemyPrefab);
            enemies.Add(e);
            e.name = "Enemy_" + (i+1);
        }
        foreach (GameObject e in enemies)
        {
            float posX = Random.Range(xLEFT, xRIGHT);
            float posY = Random.Range(yBOTTOM, yTOP);
            while ((posX < player.GetComponent<Rigidbody2D>().position.x+5 && posX > player.GetComponent<Rigidbody2D>().position.x-5) || (posY < player.GetComponent<Rigidbody2D>().position.y+5 && posY > player.GetComponent<Rigidbody2D>().position.y-5))
            {
                posX = Random.Range(xLEFT, xRIGHT);
                posY = Random.Range(yBOTTOM, yTOP);
            }
            e.transform.position = new Vector2 (posX, posY);
        }
    }
}
