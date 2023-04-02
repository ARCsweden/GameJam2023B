using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public IEnumerator OnPlayerJoined(PlayerInput playerInput)
    {
        Player player = playerInput.gameObject.GetComponent<Player>();
        GameObject scoreUI = GameObject.Instantiate<GameObject>(playerScorePrefab, UIContainer.transform);
        Image image = scoreUI.GetComponentInChildren<Image>();
        Material avatarMat = image.material;
        // TODO: Player color is not set in Start yet
        yield return new WaitUntil(() => player.IsInitialized);
        avatarMat.SetColor("_Color", player.color);
    }
}
