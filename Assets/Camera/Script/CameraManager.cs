using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    int timerAtEnd;

    [SerializeField]
    int EndOfLevel;

    bool LevelEnd;

    float timer = 0;

    [SerializeField, Range(0,1)]
    float CameraSpeed;

    [SerializeField]
    Player[] Players;

    [SerializeField]
    float cameraDeadZone;

    // Start is called before the first frame update
    void Start()
    {
        LevelEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        if (!LevelEnd)
        {
            Players = FindObjectsOfType<Player>();
            float maxX = 0;
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].GetComponent<Transform>().position.x > maxX)
                {
                    maxX = Players[i].GetComponent<Transform>().position.x;
                }
            }
            float actualCameraSpeed = CameraSpeed * Mathf.Abs(maxX - transform.position.x);
            actualCameraSpeed = Mathf.Clamp(actualCameraSpeed, 0, 1);

            if (maxX > transform.position.x + cameraDeadZone)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Mathf.Max(actualCameraSpeed, CameraSpeed));
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), CameraSpeed);
            }

            if (transform.position.x > EndOfLevel)
            {
                LevelEnd = true;

            }
        }
        else
        {
            timer += Time.deltaTime;
        }


        if (timer > timerAtEnd)
        {
            foreach (var item in Players)
            {
                //item.Alive = false;
                item.gameObject.transform.position = new Vector3(0, item.gameObject.transform.position.y, 0);
                item.score = 0;
                item.UpdateSccore();
            }
            transform.position = new Vector3(0, 0, transform.position.z);
            timer = 0;
            LevelEnd = false;

            

        }      
    }
}
