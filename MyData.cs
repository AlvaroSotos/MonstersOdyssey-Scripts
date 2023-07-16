using System;
using UnityEngine;

[Serializable]
public class MyData 
{

    // script Álvaro 
    [Serializable]
    public struct PlayerStats
    {
        public int current_life;
        public string name;
        public float x;
        public float y;
        public float z;
        public Vector3 position;
        public Quaternion rotation;
     
    }

    [SerializeField]
    public PlayerStats stats;
}
