using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    List<Player> players = new List<Player>();

    public SpriteRenderer winnerSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Count > 0)
        {
            Player winner = players[0];
            int maxScore = 0;
            foreach (Player p in players)
            {
                if (p.score > maxScore)
                {
                    winner = p;
                    maxScore = p.score;
                }
            }

            winnerSprite.material.SetColor("_Color", winner.color);
        }
    }

    public void RegisterPlayer(Player player)
    {
        players.Add(player);
    }
}
