using System.Collections;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    const string Key = "GameData";

    [SerializeField] bool isAutoSaveOnLoad = false;
    [SerializeField] float autoSaveInterval = 5f;
    [SerializeField] GameData gameData;

    bool isLoaded;
    IEnumerator autoSaveCoroutine;
    WaitForSeconds waitAutoSave;

    public bool IsLoaded => isLoaded;

    // Dùng GameData.Instance sẽ ngắn gọn hơn
    public GameData GameData
    {
        get
        {
            if (!isLoaded)
            {
                Debug.LogError("Lỗi truy xuất data trước khi load");
                return null;
            }

            return gameData;
        }
    }

    private void Start()
    {
        waitAutoSave = new WaitForSeconds(autoSaveInterval);
        autoSaveCoroutine = AutoSaveRoutine();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause && isLoaded)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        if (isLoaded)
        {
            SaveData();
        }
    }

    public void LoadData()
    {
        if (isLoaded)
        {
            return;
        }

        if (!PlayerPrefs.HasKey(Key))
        {
            gameData = new GameData();
        }
        else
        {
            gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(Key));
        }

        if (isAutoSaveOnLoad)
        {
            StartAutoSave();
        }

        isLoaded = true;
    }

    public void SaveData()
    {
        if (isLoaded)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(gameData));
            PlayerPrefs.Save();
        }
    }

    public void StartAutoSave()
    {
        if (autoSaveCoroutine != null)
        {
            StartCoroutine(autoSaveCoroutine);
        }
    }

    public void StopAutoSave()
    {
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
    }

    IEnumerator AutoSaveRoutine()
    {
        yield return waitAutoSave;

        SaveData();
    }
}
