using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    float timer;

    float lastSpawn;

    float newSpawnIn;

    [SerializeField, Range(10,30)]
    float minSpawnRange;

    [SerializeField, Range(30,80)]
    float maxSpawnRange;

    [SerializeField]
    GameObject[] buildings;

    // Start is called before the first frame update
    void Start()
    {
        buildings = Resources.LoadAll<GameObject>("Buildings");
        lastSpawn = transform.position.x;
        newSpawnIn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (newSpawnIn < (transform.position.x - lastSpawn))
        {
            GameObject newSpawn = GameObject.Instantiate(buildings[Random.Range(0,buildings.Length)]);
            newSpawn.transform.position = transform.position;
            lastSpawn = transform.position.x;
            newSpawnIn = Random.Range(minSpawnRange, maxSpawnRange);
        }
    }

}
