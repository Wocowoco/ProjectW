using Secrets;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DiscordWebhook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendDiscord(string message)
    {
        StartCoroutine(SendWebHook(Secret.WEBHOOK_LINK_DISCORD, message, (success) =>
        {
            if (success == true)
            {
                Debug.Log("Message Sent");
            }
        })) ;
    }

    IEnumerator SendWebHook(string link, string message, System.Action<bool> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);
        using (UnityWebRequest www = UnityWebRequest.Post(link, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                action(false);
            }
            else
            {
                action(true);
            }
        }
    }
}
