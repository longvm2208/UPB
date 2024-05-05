using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private const string Key = "GameData";

    [SerializeField] private GameData gameData;

    private bool isLoaded;

    public bool IsLoaded => isLoaded;
    public GameData GameData => gameData;

    private void OnApplicationPause(bool pause)
    {
        if (pause && isLoaded)
        {
            SaveData();
        }
    }

    private void OnApplicationQuit()
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
}
