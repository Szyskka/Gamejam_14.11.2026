using UnityEngine;

public class set_map_script : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private GameObject player;
    
    public void SetMap()
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
}
