using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Text;

public class GameController : MonoBehaviour
{
    public GameObject backpack;
    backpack bp;

    public TMP_Text torusCount;
    public TMP_Text sphereCount;
    public TMP_Text boxCount;

    // Start is called before the first frame update
    void Start()
    {
        bp = backpack.GetComponent<backpack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToInv()
    {
        Debug.Log("add");
        updateCounters();
        StartCoroutine(MakeRequest("add"));
    }

    public void removeFromInv() {
        Debug.Log("rem");
        updateCounters();
        StartCoroutine(MakeRequest("remove"));
    }

    public void updateCounters() {
        torusCount.text = bp.torusCounter + "";
        sphereCount.text = bp.sphereCounter + "";
        boxCount.text = bp.boxCounter + "";
    }

    private IEnumerator MakeRequest(string action)
    {
        // POST request creation
        var dataToPost = new PostData() { action = action, itemID = bp.lastObject.GetComponent<movableObject>().id };
        var postRequest = CreateRequest("https://wadahub.manerai.com/api/inventory/status", RequestType.POST, dataToPost);
        AttachHeader(postRequest, "Bearer Token", "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP");
        yield return postRequest.SendWebRequest();
        var deserializedPostData = JsonUtility.FromJson<PostResult>(postRequest.downloadHandler.text);
        Debug.Log(postRequest.downloadHandler.text);
        // Trigger continuation of game flow
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }
}
public enum RequestType
{
    GET = 0,
    POST = 1,
    PUT = 2
}


public class Todo
{
    // Ensure no getters / setters
    // Typecase has to match exactly
    public int userId;
    public int id;
    public string title;
    public bool completed;
}

[Serializable]
public class PostData
{
    public string action;
    public int itemID;
}

public class PostResult
{
    public string success { get; set; }
}
