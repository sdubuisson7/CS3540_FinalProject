using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SettingsMenuBehavior : MonoBehaviour
{
    public Slider cameraSensitivitySlider;
    float rotationSpeed;
    public TextMeshProUGUI sensitivity;


    private void Start()
    {
        rotationSpeed = PlayerPrefs.GetFloat("rotationSpeed", 300);
        cameraSensitivitySlider.value = rotationSpeed;
        sensitivity.text = rotationSpeed.ToString();

    }

    void Update() 
    {
        rotationSpeed = cameraSensitivitySlider.value;
        sensitivity.text = rotationSpeed.ToString();
        PlayerPrefs.SetFloat("rotationSpeed", rotationSpeed);


    }

    
}
