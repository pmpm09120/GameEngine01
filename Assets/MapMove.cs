using UnityEngine;

public class MapMove : MonoBehaviour
{
    [SerializeField] private Transform map1, map2;
    public float movespeed = 5f;
    [SerializeField] private float endy;
    [SerializeField] private float mapHeight;


    private void Update()
    {
        map1.transform.Translate(Vector2.down * movespeed * Time.deltaTime, Space.World);
        map2.transform.Translate(Vector2.down * movespeed * Time.deltaTime, Space.World);

        if (map1.transform.position.y <= endy)
        {
            map1.transform.position = new Vector3(map2.transform.position.x, map2.transform.position.y + mapHeight, map2.transform.position.z);
        }
        
        if(map2.transform.position.y <= endy)
        {
            map2.transform.position = new Vector3(map1.transform.position.x, map1.transform.position.y + mapHeight, map1.transform.position.z);
        }
    }
}
