using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PathData", menuName = "ScriptAbleObject/PathData")]
public class PathData : ScriptableObject
{
    public int pathID;
    public List<Vector3> waypoints;
}
