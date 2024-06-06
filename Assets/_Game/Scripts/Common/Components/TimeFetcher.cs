using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeFetcher : MonoBehaviour
{
    [SerializeField] private string url = "https://www.google.com";

    public void FetchTimeFromServer(int timeout = 3, Action<DateTime> onComplete = null)
    {
        StartCoroutine(Routine(timeout, onComplete));

        IEnumerator Routine(int timeout, Action<DateTime> onComplete)
        {
            DateTime time;
            UnityWebRequest request = new UnityWebRequest(url);
            request.timeout = timeout;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string date = request.GetResponseHeaders()["date"];
                time = DateTime.Parse(date);
            }
            else
            {
                Debug.LogWarning("Using local time");
                time = DateTime.Now;
            }

            onComplete?.Invoke(time);
        }
    }
}
