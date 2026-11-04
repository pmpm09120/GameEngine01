using UnityEngine;
using System.Collections.Generic;


public class PathGenerator : MonoBehaviour
{
    ///<summary>
    ///PathGenerator(싱글톤)
    ///
    /// >GeneratePath 호출
    /// ID:waypoints로 구분된 Dictionary 데이터 찾기
    /// id에 맞는 path waypoints를 좌표에 맞게 객체를 생성
    /// 생성된 waypoints는 요청자의 Transform 타입의 리스트에 보냄
    ///<summary>
    ///
    public static PathGenerator Instance { get; private set; }

    public List<PathData> paths = new List<PathData>();
    Dictionary<int, List<Vector3>> pathDictionary = new Dictionary<int, List<Vector3>>();

    public GameObject pathParent;
    public GameObject waypointObj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pathParent = GameObject.Find("PathParent");

        InitalizeData();
    }

    private void InitalizeData()
    {
        if (paths.Count <= 0) return;

        foreach (var path in paths)
        {
            int key = path.pathID;
            if (pathDictionary.ContainsKey(key)) { continue; }
            pathDictionary.Add(key, path.waypoints);
        }
        Debug.Log($"[PathGenerator] pathDictionary 데이터 초기화 완료 / pathDictionary Count: {pathDictionary.Count}");
    }
    public List<Transform> GeneratePath(int pathID)
    {   
        if (!pathDictionary.ContainsKey(pathID))
        {
            Debug.LogWarning($"[PathGenerator] Path ID: {pathID}를 목록에서 찾을 수 없습니다.");
            return null;
        }

        List<Transform> waypointsPos = new List<Transform>();
        
        if (pathDictionary.TryGetValue(pathID, out List<Vector3> waypoints))
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                GameObject instance = Instantiate(waypointObj, waypoints[i], Quaternion.identity, pathParent.transform);
                waypointsPos.Add(instance.transform);
            }
            Debug.Log($"[PathGenerator] 웨이포인트 생성 성공 / waypoint Count: {waypointsPos.Count}");
        }
        return waypointsPos;
    }
}
