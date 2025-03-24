using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int playersAlive = 2;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDied(int playerNumber)
    {
        playersAlive--;

        Debug.Log($"Jugador {playerNumber} ha muerto. Jugadores restantes: {playersAlive}");

        if (playersAlive <= 0)
        {
            Debug.Log("¡Empate! Ambos jugadores han muerto.");
            EndGame();
        }
        else if (playersAlive == 1)
        {
            int winner = playerNumber == 1 ? 2 : 1;
            Debug.Log($"¡Jugador {winner} gana la partida!");
            EndGame();
        }
    }

    private void EndGame()
    {
        Invoke(nameof(RestartGame), 3f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
