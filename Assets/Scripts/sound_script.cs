using UnityEngine;

public class sound_script : MonoBehaviour
{
    public AudioClip shootProjctile;
    public AudioClip dieSound;
    public AudioClip enemyDeath;
    public AudioSource audioSource;
    
    public static sound_script Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootProjctile);
    }
    public void PlayDieSound()
    {
        audioSource.PlayOneShot(dieSound);
    }
    public void PlayOnEnemyDeath()
    {
        audioSource.PlayOneShot(enemyDeath);
    }
}
