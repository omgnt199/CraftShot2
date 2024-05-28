using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TestEffect : MonoBehaviour
{
    [SerializeField] GameObject BlockPrefab;
    private List<GameObject> listBlocks;
    // Start is called before the first frame update
    void Start()
    {
        listBlocks = new List<GameObject>();
        CreateBlock();
        Invoke("EplosionBlock", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateBlock()
    {
        for (float x = 0; x <= 4f; x++)
            for (float y = 0; y <= 4f; y++)
                for (float z = 0; z <= 4f; z++)
                {
                    GameObject temp = Instantiate(BlockPrefab, new Vector3(x, y, z), Quaternion.identity);
                    listBlocks.Add(temp);
                }
    }
    void EplosionBlock()
    {
        foreach(GameObject temp in listBlocks)
        {
            float explosionForce, explosionRadius;
            Rigidbody rb = temp.GetComponent<Rigidbody>();
            explosionForce = 500f;
            explosionRadius = 10f;
            rb.AddExplosionForce(explosionForce, new Vector3(2.5f, 2.5f, 2.5f), explosionRadius);
        }
    }
}
