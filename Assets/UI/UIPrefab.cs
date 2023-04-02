using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIPrefab : MonoBehaviour
{
    public GameObject UIContainer;
    public GameObject playerScorePrefab;

    const int playerHeight = 30;

    int numPlayers = 0;

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
        // TODO: Offset is not entirely correct, unclear why
        numPlayers++;
        // Resize the UI container panel to make space for the new player
        RectTransform tf = UIContainer.GetComponent<RectTransform>();
        tf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, numPlayers * playerHeight);

        // Create a new scoreboard element and add it to the UI container
        Player player = playerInput.gameObject.GetComponent<Player>();
        Transform UITransform = UIContainer.transform;
        GameObject scoreUI = GameObject.Instantiate<GameObject>(playerScorePrefab, UITransform);
        //scoreUI.transform.position += new Vector3(0, (numPlayers - 1) * playerHeight, 0);
        Image image = scoreUI.GetComponentInChildren<Image>();
        Material avatarMat = image.material;
        // Player color is not set in Start yet, we have to wait until it is
        yield return new WaitUntil(() => player.IsInitialized);
        avatarMat.SetColor("_Color", player.color);
    }
}
