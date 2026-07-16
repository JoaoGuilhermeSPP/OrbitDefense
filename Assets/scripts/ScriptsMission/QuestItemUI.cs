using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    public TMP_Text questNameText;

    public TMP_Text progressText;

    public TMP_Text rewardText;

    public Slider progressBar;

    public Button claimButton;

    MissionData missionData;

    MissionProgress missionProgress;

    public void Setup(
        MissionData data,
        MissionProgress progress)
    {
        missionData =
            data;

        missionProgress =
            progress;

        claimButton.onClick
            .RemoveAllListeners();

        claimButton.onClick
            .AddListener(
                ClaimReward);

        progressBar.interactable =
            false;

        UpdateUI();
    }

    public void UpdateUI()
    {
        questNameText.text =
            missionData.name;

        progressText.text =
            missionProgress.currentValue
            + "/"
            + missionData.TargetValue;

        rewardText.text =
            missionData.rewardCoins
            .ToString();

        progressBar.maxValue =
            missionData.TargetValue;

        progressBar.value =
            missionProgress.currentValue;

        claimButton.gameObject
            .SetActive(
                missionProgress.completed
                &&
                !missionProgress.claimed
            );
    }

    void ClaimReward()
    {
        MissionManager.instance
            .ClaimReward(
                missionProgress);

        Destroy(gameObject);
    }
}