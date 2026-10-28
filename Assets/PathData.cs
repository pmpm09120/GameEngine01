using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New PathData", fileName = "ScriptAbleObjcet/PathData")]
public class PathData : ScriptableObject
{
    [SerializeField] private int pathID;
    public List<Transform> waypoints;

}
