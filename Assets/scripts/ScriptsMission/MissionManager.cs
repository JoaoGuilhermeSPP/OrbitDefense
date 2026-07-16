using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [Header("Database")]
    public MissionDataBase dataBase;

    public List<MissionProgress> ProgressList
    {
        get
        {
            if (SaveManager.instance == null)
                return null;

            if (SaveManager.instance.data == null)
                return null;

            if (SaveManager.instance.data.missionProgress == null)
            {
                SaveManager.instance.data.missionProgress = new List<MissionProgress>();
            }

            return SaveManager.instance.data.missionProgress;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Start()
    {
        StartCoroutine(InicializarComSeguranca());
    }

    private IEnumerator InicializarComSeguranca()
    {
        // Espera o SaveManager estar pronto
        yield return new WaitUntil(() => SaveManager.instance != null);
        yield return null;

        CreateProgressList();
    }

    void CreateProgressList()
    {
        if (dataBase == null)
        {
            Debug.LogError("MissionDatabase não atribuída no MissionManager");
            return;
        }

        List<MissionProgress> progressList = ProgressList;

        if (progressList == null)
        {
            Debug.LogError("ProgressList é NULL - SaveManager não está pronto");
            return;
        }

        if (progressList.Count > 0)
            return;

        foreach (MissionData mission in dataBase.missionData)
        {
            MissionProgress progress = new MissionProgress();
            progress.id = mission.id;
            progress.currentValue = 0;
            progress.completed = false;
            progress.claimed = false;

            progressList.Add(progress);
        }

        SaveManager.instance.Save();
    }

    public void AddMissionProgress(Mission type, int amount)
    {
        if (SaveManager.instance == null)
        {
            Debug.LogWarning("SaveManager.instance é null em AddMissionProgress");
            return;
        }

        List<MissionProgress> progressList = ProgressList;

        if (progressList == null)
            return;

        foreach (MissionProgress progress in progressList)
        {
            if (progress == null)
                continue;

            MissionData data = GetMissionData(progress.id);

            if (data == null)
                continue;

            if (data.missionType != type)
                continue;

            if (progress.completed)
                continue;

            progress.currentValue += amount;

            if (progress.currentValue >= data.TargetValue)
            {
                progress.currentValue = data.TargetValue;
                progress.completed = true;
                Debug.Log("MISSÃO COMPLETA: " + data.name);
            }
        }

        SaveManager.instance.Save();
    }

    public void ClaimReward(MissionProgress progress)
    {
        if (progress == null)
            return;

        if (!progress.completed)
            return;

        if (progress.claimed)
            return;

        MissionData data = GetMissionData(progress.id);

        if (data == null)
            return;

        SaveManager.instance.AddCoins(data.rewardCoins);

        progress.claimed = true;

        SaveManager.instance.Save();
    }

    public MissionData GetMissionData(string id)
    {
        if (dataBase == null)
            return null;

        return dataBase.missionData.Find(x => x.id == id);
    }

    public static void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}