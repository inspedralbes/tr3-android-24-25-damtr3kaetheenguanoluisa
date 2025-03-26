// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using TMPro;
// using MyGame.Models;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance { get; private set; }
    
//     [Header("Player Information")]
//     public int player1Id;
//     public int player2Id;
//     public string player1Username;
//     public string player2Username;
//     public int player1Bombs;
//     public int player2Bombs;
//     public int player1Kills;
//     public int player2Kills;
//     public int player1Victories;
//     public int player2Victories;

//     [Header("UI Elements")]
//     public TextMeshProUGUI winnerText;
//     public GameObject gameOverPanel;
//     public GameObject gamePanel;

//     private string apiUrl = "http://localhost:3020/players";

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         LoadPlayersData();
//         StartCoroutine(RefreshPlayersDataFromServer());
        
//         if (gamePanel != null) gamePanel.SetActive(true);
//         if (gameOverPanel != null) gameOverPanel.SetActive(false);
//     }

//     private void LoadPlayersData()
//     {
       
//         player1Id = PlayerPrefs.GetInt("player1Id", -1);
//         player1Username = PlayerPrefs.GetString("player1Username", "");
//         player1Bombs = PlayerPrefs.GetInt("player1Bombs", 5);
//         player1Victories = PlayerPrefs.GetInt("player1Victories", 0);
//         player1Kills = PlayerPrefs.GetInt("player1EnemiesDefeated", 0);

//         player2Id = PlayerPrefs.GetInt("player2Id", -1);
//         player2Username = PlayerPrefs.GetString("player2Username", "");
//         player2Bombs = PlayerPrefs.GetInt("player2Bombs", 5);
//         player2Victories = PlayerPrefs.GetInt("player2Victories", 0);
//         player2Kills = PlayerPrefs.GetInt("player2EnemiesDefeated", 0);

//         if (player1Id == -1 || player2Id == -1)
//         {
//             Debug.LogError("No todos los jugadores están logueados.");
//         }
//     }

//     private IEnumerator RefreshPlayersDataFromServer()
//     {
//         UnityWebRequest request1 = UnityWebRequest.Get($"{apiUrl}/{player1Id}");
//         yield return request1.SendWebRequest();

//         if (request1.result == UnityWebRequest.Result.Success)
//         {
//             PlayerData data1 = JsonUtility.FromJson<PlayerData>(request1.downloadHandler.text);
//             if (data1.success)
//             {
//                 player1Username = data1.player.username;
//                 player1Bombs = data1.player.bombs;
//                 player1Victories = data1.player.victories;
//                 player1Kills = data1.player.enemiesDefeated;

//                 PlayerPrefs.SetString("player1Username", player1Username); 
//                 PlayerPrefs.SetInt("player1Bombs", player1Bombs);
//                 PlayerPrefs.SetInt("player1Victories", player1Victories);
//                 PlayerPrefs.SetInt("player1EnemiesDefeated", player1Kills);
//             }
//         }

//         UnityWebRequest request2 = UnityWebRequest.Get($"{apiUrl}/{player2Id}");
//         yield return request2.SendWebRequest();

//         if (request2.result == UnityWebRequest.Result.Success)
//         {
//             PlayerData data2 = JsonUtility.FromJson<PlayerData>(request2.downloadHandler.text);
//             if (data2.success)
//             {
//                 player2Bombs = data2.player.bombs;
//                 player2Victories = data2.player.victories;
//                 player2Kills = data2.player.enemiesDefeated;
                
//                 PlayerPrefs.SetInt("player2Bombs", player2Bombs);
//                 PlayerPrefs.SetInt("player2Victories", player2Victories);
//                 PlayerPrefs.SetInt("player2EnemiesDefeated", player2Kills);
//             }
//         }

//         PlayerPrefs.Save();
//         Debug.Log("Datos de jugadores actualizados desde el servidor");
//     }

//     public void PlayerDied(int playerId)
//     {
//         if (playerId == player1Id)
//         {
//             ShowWinner(player2Username);
//             StartCoroutine(UpdatePlayerStats(player2Id, player2Kills, true));
//             StartCoroutine(UpdatePlayerStats(player1Id, player1Kills, false));
//         }
//         else if (playerId == player2Id)
//         {
//             ShowWinner(player1Username);
//             StartCoroutine(UpdatePlayerStats(player1Id, player1Kills, true));
//             StartCoroutine(UpdatePlayerStats(player2Id, player2Kills, false));
//         }
//     }

//     private void ShowWinner(string winnerUsername)
//     {
//         if (gameOverPanel != null && winnerText != null)
//         {
//             gamePanel.SetActive(false);
//             gameOverPanel.SetActive(true);
//             winnerText.text = $"¡{winnerUsername} ha ganado la partida!";
//         }
//     }

//     private IEnumerator UpdatePlayerStats(int playerId, int kills, bool isWinner)
//     {
//         string url = $"{apiUrl}/updateStats/{playerId}";
//         WWWForm form = new WWWForm();
//         form.AddField("username", playerId == player1Id ? player1Username : player2Username);
//         form.AddField("kills", kills);
//         form.AddField("isWinner", isWinner ? 1 : 0);

//         UnityWebRequest request = UnityWebRequest.Post(url, form);
//         request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.LogError($"Error actualizando estadísticas: {request.error}");
//         }
//         else
//         {
//             Debug.Log($"Estadísticas actualizadas correctamente para el jugador {playerId}");
            
//             if (playerId == player1Id && isWinner)
//             {
//                 player1Victories++;
//                 PlayerPrefs.SetInt("player1Victories", player1Victories);
//             }
//             else if (playerId == player2Id && isWinner)
//             {
//                 player2Victories++;
//                 PlayerPrefs.SetInt("player2Victories", player2Victories);
//             }
//             PlayerPrefs.Save();
//         }
//     }

//     public void AddKill(int playerId)
//     {
//         if (playerId == player1Id)
//         {
//             player1Kills++;
//         }
//         else if (playerId == player2Id)
//         {
//             player2Kills++;
//         }
//     }

//     public void RestartGame()
//     {
//         // Reiniciar el juego
//         if (gamePanel != null) gamePanel.SetActive(true);
//         if (gameOverPanel != null) gameOverPanel.SetActive(false);
        
//         // Reiniciar contadores de kills
//         player1Kills = 0;
//         player2Kills = 0;
//     }
// }