using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Huaman Alice = new Huaman("alice", 23, 160);
        Debug.Log($"{Alice.name} / {Alice.age} / {Alice.height}");

        Huaman Jon = new Huaman();
    }
}

class Huaman
{
    public string name;
    public float height;
    public int age;

    public Huaman()
    {

    }

    public Huaman(string _name, int _age, float _height)
    {
        name = _name;
        age = _age;
        height = _height;
    }

    class Adult: Huaman
    {
        public Adult(string _name, int _age, float _height) : base(_name, _age, _height) { }
    }
}
