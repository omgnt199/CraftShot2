using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    [System.Serializable]
    public class BulletTypes
    {
        public string name;
        public string code;
        public GameObject prefab;
    }
    public BulletTypes[] bulletTypes;
}
