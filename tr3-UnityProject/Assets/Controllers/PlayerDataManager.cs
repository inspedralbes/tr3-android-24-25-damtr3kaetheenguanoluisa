using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance; // Singleton para acceder desde cualquier parte del código.

    public PlayerInfo player1;
    public PlayerInfo player2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerData(int playerNumber, PlayerInfo playerInfo)
    {
        if (playerNumber == 1)
            player1 = playerInfo;
        else if (playerNumber == 2)
            player2 = playerInfo;
    }
}
