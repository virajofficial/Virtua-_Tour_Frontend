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

    public static Dropdown dropdown;
    public static bool isLoading = true;
    public static string imageName;
    

    void Start()
    {
        dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate
        {
            dropdownValueChanged(dropdown);
            GetImageScript.ischanged = true;
        });
    }

    public void dropdownValueChanged(UnityEngine.UI.Dropdown sender)
    {
        Debug.Log("Changed " + sender.options[UIManager.dropdown.value].text);
        GetImageScript.roomName = sender.options[UIManager.dropdown.value].text;
        //sender.value = UIManager.dropdown.value;
    }



    void Update()
    {
        name.text = imageName;
        Loading();
        
    }

    
    

    void Loading()
    {
        if (isLoading)
        {
            loadingScreen.SetActive(true);
            buttons.SetActive(false);
            bottomPanel.SetActive(false);
            name.gameObject.SetActive(false);
            dropdown.gameObject.SetActive(false);
        }
        else
        {
            loadingScreen.SetActive(false);
            buttons.SetActive(true);
            bottomPanel.SetActive(true);
            name.gameObject.SetActive(true);
            if(GetImageScript.jsondata.picker_value == "bareland")
            {
                dropdown.gameObject.SetActive(true);
            }
            
        }
    }
}
