using MyGame.Models;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalPlayers;
    private int remainingPlayers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        totalPlayers = FindObjectsByType<BombermanController>(FindObjectsSortMode.None).Length;
        remainingPlayers = totalPlayers;
    }

    public void PlayerDied(BombermanController player)
    {
        remainingPlayers--;
        BombermanController winner = DetermineWinner();  

        if (winner != null)
        {
            winner.victories++; 
            EndGame();
        }
        else if (remainingPlayers <= 0)
        {
            EndGame();
        }
    }

    private BombermanController DetermineWinner()
    {
        BombermanController[] players = FindObjectsByType<BombermanController>(FindObjectsSortMode.None);
        foreach (BombermanController player in players)
        {
            if (player.isActiveAndEnabled)
            {
                return player;
            }
        }
        return null;
    }

    private BombermanController FindPlayer(int playerNumber)
    {
        BombermanController[] players = FindObjectsByType<BombermanController>(FindObjectsSortMode.None);
        foreach (BombermanController player in players)
        {
            if (player.playerNumber == playerNumber)
            {
                return player;
            }
        }
        return null;
    }

   public void EndGame()
{
    BombermanController player1 = FindPlayer(1);
    BombermanController player2 = FindPlayer(2);

    List<PlayerInfo> players = new List<PlayerInfo>();

    if (player1 != null)
    {
        players.Add(CreatePlayerInfo(player1));
    }

    if (player2 != null)
    {
        players.Add(CreatePlayerInfo(player2));
    }

    if (players.Count > 0)
    {
        StartCoroutine(AuthManager.Instance.SendStatsToServer(players)); 
    }
    else
    {
        Debug.LogWarning("⚠️ No hi ha jugadors vàlids per enviar dades. ");
    }
}

private PlayerInfo CreatePlayerInfo(BombermanController player)
{
    return new PlayerInfo
    {
        username = PlayerPrefs.GetString($"player{player.playerNumber}Username", "desconegut"),
        bombAmount = player.GetComponent<BombController>().bombAmount, 
        bombsUsed = player.bombsUsed,
        speed = Mathf.RoundToInt(player.speed), 
        victories = player.victories,
        enemiesDefeated = player.enemiesDefeated
    };
}
}
