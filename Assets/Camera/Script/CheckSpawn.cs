using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpawn : MonoBehaviour
{
    [SerializeField]
    Transform[] checkSpawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getSpawnPoint()
    {
        foreach (var item in checkSpawnPoints)
        {
            if (Physics.OverlapSphere(item.position,1).Length == 0)
            {
                Debug.Log("Found Spawn position at: " + item.position);
                return item.position;
            }
        }
        Debug.Log("No Spawn point found");
        return new Vector3(0, 0, 0);
    }
}
