using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPrefab : MonoBehaviour
{
    public GameObject UIContainer;
    public GameObject playerScorePrefab;


    // Start is called before the first frame update
    void Start()
    {
        //GameObject scoreUI = GameObject.Instantiate<GameObject>(playerScorePrefab, UIContainer.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerJoined()
    {
        GameObject scoreUI = GameObject.Instantiate<GameObject>(playerScorePrefab, UIContainer.transform);
    }
}
