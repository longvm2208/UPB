using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class NetworkManager : SingletonMonoBehaviour<NetworkManager>
{
#if UNITY_EDITOR
    [SerializeField] bool isInternetAvailableOnEditor = true;
#endif
    [SerializeField] bool isContinuousCheck;
    [SerializeField] float checkInterval = 1f;

    public event Action<bool> OnNetworkStatusChanged;

    bool status;
    WaitForSeconds wait;

    [Button]
    public void Initialize()
    {
        if (isContinuousCheck)
        {
            status = IsInternetAvailable();
            wait = new WaitForSeconds(checkInterval);
            StartCoroutine(CheckRoutine());
        }
    }

    IEnumerator CheckRoutine()
    {
        while (true)
        {
            yield return wait;

            if (status != IsInternetAvailable())
            {
                status = IsInternetAvailable();
                OnNetworkStatusChanged?.Invoke(status);
            }
        }
    }

    public bool IsInternetAvailable()
    {
#if UNITY_EDITOR
        return isInternetAvailableOnEditor;
#else
        return !(Application.internetReachability == NetworkReachability.NotReachable);
#endif
    }
}