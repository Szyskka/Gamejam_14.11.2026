using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class wave_manager_script : MonoBehaviour
{
    public int wave = 1;
    public TMP_Text waveText;
    public TMP_Text waveOnDeath;
    
    void Update()
    {
        waveText.text = "Wave "+ wave;
        waveOnDeath.text = "Wave "+wave;
    }
}
