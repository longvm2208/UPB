using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeFetcher : MonoBehaviour
{
    [SerializeField]
    private bool isLocalTime;

    public void FetchTimeFromServer(int timeout = 3, Action<DateTime> onComplete = null)
    {
        if (isLocalTime)
        {
            DateTime startupTime = DateTime.Now - TimeSpan.FromSeconds(Time.realtimeSinceStartup);
            onComplete?.Invoke(startupTime);
        }
        else
        {
            StartCoroutine(FetchTimeFromServerRoutine(timeout, onComplete));
        }
    }

    private IEnumerator FetchTimeFromServerRoutine(int timeout, Action<DateTime> onComplete)
    {
        DateTime startupTime;
        UnityWebRequest request = new UnityWebRequest("https://www.google.com");
        request.timeout = timeout;

        yield return request.SendWebRequest();

        bool isProtocolError = request.result == UnityWebRequest.Result.ProtocolError;
        bool isConnectionError = request.result == UnityWebRequest.Result.ConnectionError;

        if (isProtocolError || isConnectionError || request.error != null)
        {
            Debug.LogWarning("Using local time");
            startupTime = DateTime.Now;
        }
        else
        {
            string date = request.GetResponseHeaders()["date"];
            startupTime = DateTime.Parse(date);
        }

        startupTime -= TimeSpan.FromSeconds(Time.realtimeSinceStartup);

        onComplete?.Invoke(startupTime);
    }
}
