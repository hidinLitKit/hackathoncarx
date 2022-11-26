using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class FlashLighTurn : MonoBehaviour
{
    public Color OnColour;
    public float onDensity;
    private Color ambientCol;
    private float fogDensity;
    public GameObject FlashLight;
    public KeyCode FlashLightKey;
    public AudioSource FlashSnd;
    private bool isTurn = false;
    private void Awake()
    {
        ambientCol = RenderSettings.ambientLight;
        fogDensity = RenderSettings.fogDensity;
    }
    void Update()
    {
        #if UNITY_STANDALONE
        if (Input.GetKeyDown(FlashLightKey))
        {
            isTurn = !isTurn;
            FlashLight.SetActive(isTurn);
            FlashSnd.Play();
            if (isTurn) 
            {
                RenderSettings.fogDensity = onDensity;
                RenderSettings.ambientLight = OnColour;
            }
            else 
            { 
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.ambientLight = ambientCol;
            }
            
        }
        #elif UNITY_ANDROID
        if (CrossPlatformInputManager.GetButtonDown("Torch"))
        {
            isTurn = !isTurn;
            FlashLight.SetActive(isTurn);
            FlashSnd.Play();
            if (isTurn) 
            {
                RenderSettings.fogDensity = onDensity;
                RenderSettings.ambientLight = OnColour;
            }
            else 
            { 
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.ambientLight = ambientCol;
            }
            
        }
        #endif
    }
}
