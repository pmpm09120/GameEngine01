using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptAbleObject/New WaveData")]
public class WaveData : ScriptableObject
{
    public int wave_id;
    public List<SpawnTimeScript> TimeScripts = new List<SpawnTimeScript>();

    public void SortScriptsbyTime()
    {
        TimeScripts.Sort((x, y) => x.startTime.CompareTo(y.startTime));
    }
}

[Serializable]
public struct SpawnObj
{
    public int poolID;
    public int pathID;
}

[Serializable]
public struct SpawnGroup
{
    public List<SpawnObj> spawnObjs;
    public float Interval; //생성주기
}
[Serializable]
public struct SpawnTimeScript
{
    public float startTime;
    public bool isBoss;
    public List<SpawnGroup> spawnGroups;
}