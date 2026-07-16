using UnityEngine;

public class MissionUIManager : MonoBehaviour
{
    public Transform contentParent;

    public GameObject questItemPrefab;

    private void Start()
    {
        CreateMissionUI();
    }

    public void CreateMissionUI()
    {
        if (MissionManager.instance == null)
            return;

        foreach (Transform child
            in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (MissionProgress progress
            in MissionManager
            .instance
            .ProgressList)
        {
            if (progress.claimed)
                continue;

            MissionData data =
                MissionManager
                .instance
                .GetMissionData(
                    progress.id);

            if (data == null)
                continue;

            GameObject obj =
                Instantiate(
                    questItemPrefab,
                    contentParent);

            QuestItemUI ui =
                obj.GetComponent<QuestItemUI>();

            ui.Setup(
                data,
                progress);
        }
    }
}