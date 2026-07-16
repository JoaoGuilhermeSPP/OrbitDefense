using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Saves
{
    public int highScore;
    public int lastScore;
    public int totalCoins;
    public string equipedItem;
    public List<string> itemsUnlock = new List<string>();
    public List<MissionProgress> missionProgress;
}
