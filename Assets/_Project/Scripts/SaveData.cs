using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<InventorySaveData> inventory = new List<InventorySaveData>();

    public int currentDay;
    public int currentSeason;
    public float currentHour;

    public List<DaySummarySaveData> dayHistory = new List<DaySummarySaveData>();
}