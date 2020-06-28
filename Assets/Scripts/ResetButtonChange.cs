using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonChange : MonoBehaviour
{
    public GameObject resetButton;
    public Sprite newImage;
    void OnEnable()
    {
        
        resetButton.GetComponent<Image>().sprite = newImage;
    }
}
