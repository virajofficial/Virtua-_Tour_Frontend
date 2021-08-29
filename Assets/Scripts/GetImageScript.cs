using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetImageScript : MonoBehaviour
{
    int urlLength;
    int index;
    jsonData jsondata;
    //string uri = "http://localhost:4000/";
    string uri = "https://virtual360tour-backend.herokuapp.com/";

    void Start()
    {
        StartCoroutine(DownloadImage());
    }
    
    void Update()
    {

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
                Debug.Log(jsondata);
                //TextureURL = jsondata.urls[0];
                urlLength = jsondata.urls.Length;
                index = 0;
                //Debug.Log("length : " + urlLength);
            }
            StartCoroutine(loadTexture(jsondata.urls[0]));
        }

    }

    IEnumerator loadTexture(string texURL)
    {
        UIManager.imageName = texURL.Split('_')[2].Split('.')[0] + " Room";
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
        }
    }

    public void ChangeImage(int id)
    {
        UIManager.isLoading = true;
        if(id == 0)
        {
            Debug.Log("length : " + urlLength + "\nindex : " + index);
            index++;
            if (index >= urlLength)
            {
                index = 0;
            }
        }
        else if(id == 1){
            index--;
            if(index < 0)
            {
                index = urlLength - 1;
            }
        }
        
        StartCoroutine(loadTexture(jsondata.urls[index]));
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
}
