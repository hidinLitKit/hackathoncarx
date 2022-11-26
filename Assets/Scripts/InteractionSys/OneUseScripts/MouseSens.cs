using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MouseSens : MonoBehaviour
{

    PlayerData playerData = new PlayerData();
    public Slider sensitivitySlider;
    void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/cfg.json");
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
        sensitivitySlider.value = playerData.sens;
        if (playerData.sens==0f) sensitivitySlider.value = 5f;
    }
    public void ApplySensitivity()
    {
        playerData.sens = sensitivitySlider.value;
        string jsMouse = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.dataPath + "/cfg.json", jsMouse);
        Debug.Log(jsMouse);
    }
    private class PlayerData
    {
        public float sens;
    }
}
