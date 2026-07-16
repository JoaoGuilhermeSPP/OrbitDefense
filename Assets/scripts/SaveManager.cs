using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public Saves data = new Saves();
    public static System.Action OnCoinsChanged;

    private const string SAVE_KEY = "save";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    private void OnDestroy()
    {
        // Limpa referência estática se for a instância atual
        if (instance == this)
        {
            instance = null;
        }
    }

    public void AddCoins(int amount)
    {
        data.totalCoins += amount;
        Save();
        OnCoinsChanged?.Invoke();
    }

    public void RemoveCoins(int amount)
    {
        data.totalCoins -= amount;

        if (data.totalCoins < 0)
            data.totalCoins = 0;

        Save();
        OnCoinsChanged?.Invoke();
    }

    public void SaveScore(int score)
    {
        data.lastScore = score;

        if (score > data.highScore)
        {
            data.highScore = score;
        }

        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            data = JsonUtility.FromJson<Saves>(json);
        }
    }

    // Método para destruir o SaveManager quando necessário
    public static void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}