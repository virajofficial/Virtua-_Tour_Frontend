using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetImageScript : MonoBehaviour
{
    int urlLength;
    int imageIndex;
    public static jsonData jsondata;
    List<string> options;
    public static bool ischanged = false;
    public static string roomName;

    string uri = "http://localhost:4000/";
    //string uri = "https://virtual360tour-backend.herokuapp.com/";


    void Start()
    {
        options = new List<string>();
        
        StartCoroutine(DownloadImage());
        UIManager.dropdown.ClearOptions();
    }
    
    void Update()
    {
        if (ischanged)
        {
            selectedRoomIndex(roomName);
            Debug.Log(roomName);
            ischanged = false;
        }
    }

    
    IEnumerator DownloadImage()
    {
        UIManager.isLoading = true;
        using (UnityWebRequest req = UnityWebRequest.Get(uri + "getImage/"))
        {
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log(req.error);
            }
            else
            {
                jsondata = JsonUtility.FromJson<jsonData>(req.downloadHandler.text);
                Debug.Log(req.downloadHandler.text);
                urlLength = jsondata.urls.Length;
                imageIndex = 0;
                Debug.Log("length : " + jsondata.urls.Length);
                
            }
            StartCoroutine(loadTexture(jsondata.urls[0]));
        }

    }

    IEnumerator loadTexture(string texURL)
    {
        UIManager.imageName = texURL.Split('_')[2].Split('.')[0] + ((jsondata.picker_value == "bareland") ? " Room" : "") ;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(texURL);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            
            Debug.Log(request.error);
            UIManager.isLoading = false;
        }
        else
        {
            
            Debug.Log("successfully downloaded");
            
            this.gameObject.GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            UIManager.isLoading = false;
            Debug.Log(jsondata.picker_value);
            if (jsondata.picker_value == "bareland")
            {
                Debug.Log(jsondata.urls.Length);
                loadOptions();
            }
        }
    }

    public void loadOptions()
    {
        options.Clear();
        for (int i = 0; i < jsondata.urls.Length; i++)
        {
            Debug.Log("room " + i + ": " + jsondata.urls[i]);
            options.Add(jsondata.urls[i].Split('_')[2].Split('.')[0]);
            
            
        }
        UIManager.dropdown.AddOptions(options);
    }

    public void ChangeImage(int id)
    {
        UIManager.isLoading = true;
        if(id == 0)
        {
            Debug.Log("length : " + urlLength + "\nindex : " + imageIndex);
            imageIndex++;
            if (imageIndex >= urlLength)
            {
                imageIndex = 0;
            }
        }
        else if(id == 1){
            imageIndex--;
            if(imageIndex < 0)
            {
                imageIndex = urlLength - 1;
            }
        }
        
        StartCoroutine(loadTexture(jsondata.urls[imageIndex]));
    }

    public void selectedRoomIndex(string name)
    {
        for (int i = 0; i < jsondata.urls.Length; i++)
        {
            if (jsondata.urls[i].Split('_')[2].Split('.')[0] == name)
            {
                UIManager.isLoading = true;
                StartCoroutine(loadTexture(jsondata.urls[i]));
                Debug.Log("room index :" + i);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quit...");
        OnClose();
    }


    public void OnClose()
    {
        StartCoroutine(RemoveImages());
    }

    IEnumerator RemoveImages()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(uri + "removeImages/"))
        {
            yield return req.SendWebRequest();
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log(req.error);
            }
            else
            {
                jsondata = JsonUtility.FromJson<jsonData>(req.downloadHandler.text);
                Debug.Log(req.downloadHandler.text);
            }
        }
    }
}

[SerializeField]
public class jsonData
{
    public string message;
    public string[] urls;
    public string picker_value;
}
