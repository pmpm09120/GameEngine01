using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New PathData", fileName = "PathData/ScriptAbleObjcet")]
public class PathData : ScriptableObject
{
    public int pathID;
    public List<Vector3> waypoints;
}
