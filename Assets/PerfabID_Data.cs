using UnityEngine;

[CreateAssetMenu(fileName = "New PerfabData", menuName = "ScriptAbleObject/PerfabData")]
public class PerfabID_Data : ScriptableObject
{
    public string perfabName;
    public int id;
    public GameObject perfab;
}
