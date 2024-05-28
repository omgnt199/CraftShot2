using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapManager : MonoBehaviour
{
    public int Row, Col;
    public GameObject[] CellPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateMap()
    {
        Vector3 pos = Vector3.zero;
        for (int x = 0; x < Row; x++)
        {
            for (int z = 0; z < Col; z++)
            {
                Instantiate(CellPrefab[(z + x) % 2], new Vector3(x * 50f, 0, z * -50f), Quaternion.identity, transform);
            }
        }
    }
}
