using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject buttons;
    public TextMeshProUGUI name;
    public GameObject bottomPanel;


    public static bool isLoading = true;
    public static string imageName;

    void Start()
    {

    }
    
    void Update()
    {
        name.text = imageName;
        LoadingOption();
    }

    void LoadingOption()
    {
        if (isLoading)
        {
            loadingScreen.SetActive(true);
            buttons.SetActive(false);
            bottomPanel.SetActive(false);
            name.gameObject.SetActive(false);
        }
        else
        {
            loadingScreen.SetActive(false);
            buttons.SetActive(true);
            bottomPanel.SetActive(true);
            name.gameObject.SetActive(true);
        }
    }
}
