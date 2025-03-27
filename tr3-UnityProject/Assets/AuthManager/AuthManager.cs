using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using MyGame.Models;
using System.Collections.Generic;

public class AuthManager : MonoBehaviour
{
    [Header("Player 1 Login")]
    public TMP_InputField usernameLoginInputP1;
    public TMP_InputField passwordLoginInputP1;
    private bool isPlayer1LoggedIn = false;

    [Header("Player 2 Login")]
    public TMP_InputField usernameLoginInputP2;
    public TMP_InputField passwordLoginInputP2;
    private bool isPlayer2LoggedIn = false;

    [Header("Player 1 Register")]
    public TMP_InputField usernameRegisterInputP1;
    public TMP_InputField emailRegisterInputP1;
    public TMP_InputField passwordRegisterInputP1;

    [Header("Player 2 Register")]
    public TMP_InputField usernameRegisterInputP2;
    public TMP_InputField emailRegisterInputP2;
    public TMP_InputField passwordRegisterInputP2;

    public TextMeshProUGUI messageText;
    private string apiUrl = "http://localhost:3020/players";
    public static AuthManager instance;
    private string updateStatsURL = "http://localhost:3020/players/updateUsers"; 

    public static AuthManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    public void RegisterPlayer1()
    {
        Debug.Log("Registrando jugador 1...");
        StartCoroutine(RegisterCoroutine(usernameRegisterInputP1.text, emailRegisterInputP1.text, passwordRegisterInputP1.text, 1));
    }

    public void RegisterPlayer2()
    {
        Debug.Log("Registrant jugador2...");
        StartCoroutine(RegisterCoroutine(usernameRegisterInputP2.text, emailRegisterInputP2.text, passwordRegisterInputP2.text, 2));
    }

    public void LoginPlayer1()
    {
        Debug.Log("Iniciant sessió jugador1...");
        StartCoroutine(LoginCoroutinePlayer1(usernameLoginInputP1.text, passwordLoginInputP1.text));
    }

    public void LoginPlayer2()
    {
        Debug.Log("Iniciant sessió jugador2...");
        StartCoroutine(LoginCoroutinePlayer2(usernameLoginInputP2.text, passwordLoginInputP2.text));
    }

    private IEnumerator RegisterCoroutine(string username, string email, string password, int playerNumber)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError($"Falten dades a registrar del jugador {playerNumber}");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post($"{apiUrl}/register", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PlayerData response = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
            if (response.success)
            {
                messageText.text = $"¡Jugador {playerNumber} registrat correctament!";
                Debug.Log($"Jugador {playerNumber} registrat: {username}");
            }
            else
            {
                messageText.text = response.message ?? $"Error al registrar Jugador {playerNumber}";
                Debug.LogError($"Error registre Jugador {playerNumber}: {response.message}");
            }
        }
        else
        {
            messageText.text = $"Error al registrar Jugador {playerNumber}: {request.error}";
            Debug.LogError($"Error registre Jugador {playerNumber}: {request.error}");
        }
    }

    private IEnumerator LoginCoroutinePlayer1(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post($"{apiUrl}/login", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PlayerData data = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
            if (data.success)
            {
                isPlayer1LoggedIn = true;
                Debug.Log("Login jugador 1!");

                PlayerPrefs.SetInt("player1Id", data.player.id);
                PlayerPrefs.SetString("player1Username", data.player.username);
                PlayerPrefs.SetString("player1Bombs", data.player.bombAmount.ToString());
                PlayerPrefs.SetInt("player1BombsUsed", data.player.bombsUsed);
                PlayerPrefs.SetInt("player1Victories", data.player.victories);
                PlayerPrefs.SetInt("player1EnemiesDefeated", data.player.enemiesDefeated);
                PlayerPrefs.Save();

                messageText.text = "Jugador 1 conectat correctament!";
                StartCoroutine(LoadStatsFromServer(data.player.id));
            }
            else
            {
                messageText.text = data.message;
                Debug.LogError($"Error login Jugador 1: {data.message}");
            }
        }
        else
        {
            messageText.text = "Error de conexión";
            Debug.LogError($"Error de connexió Jugador 1: {request.error}");
        }
    }

    private IEnumerator LoginCoroutinePlayer2(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post($"{apiUrl}/login", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PlayerData data = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
            if (data.success)
            {
                isPlayer2LoggedIn = true;
                Debug.Log("Login jugador 2!");

                PlayerPrefs.SetInt("player2Id", data.player.id);
                PlayerPrefs.SetString("player2Username", data.player.username);
                PlayerPrefs.SetInt("player2Bombs", data.player.bombAmount);
                PlayerPrefs.SetInt("player2BombsUsed", data.player.bombsUsed);
                PlayerPrefs.SetInt("player2Victories", data.player.victories);
                PlayerPrefs.SetInt("player2EnemiesDefeated", data.player.enemiesDefeated);
                PlayerPrefs.Save();

                messageText.text = "Jugador 2 conectat correctament!";
                StartCoroutine(LoadStatsFromServer(data.player.id));
            }
            else
            {
                messageText.text = data.message;
                Debug.LogError($"Error login Jugador 2: {data.message}");
            }
        }
        else
        {
            Debug.LogError($"Error en el login del jugador 2: {request.error}");
            messageText.text = "Error en la connexió";
        }
    }

    private IEnumerator LoadStatsFromServer(int playerId)
    {
        string url = $"{apiUrl}/{playerId}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseData = www.downloadHandler.text;
                Debug.Log($"📥 JSON recibido del servidor:\n{responseData}");

                try 
                {
                    PlayerData data = JsonUtility.FromJson<PlayerData>(responseData);
                    
                    if (data == null || data.player == null)
                    {
                        Debug.LogError("❌ Error: Dades  jugador nules");
                        yield break;
                    }

                    Debug.Log($"Objeto PlayerData:\n" +
                        $"Success: {data.success}\n" +
                        $"Message: {data.message}\n" +
                        $"Player: {data.player.username}");

                    int defaultBombs = 5;
                    int defaultSpeed = 5;

                    int bombAmount = data.player.bombs > 0 ? data.player.bombs : defaultBombs;
                    int speed = data.player.speed > 0 ? data.player.speed : defaultSpeed;

                    PlayerPrefs.SetInt($"player{playerId}Bombs", bombAmount);
                    PlayerPrefs.SetInt($"player{playerId}BombsUsed", data.player.bombsUsed);
                    PlayerPrefs.SetInt($"player{playerId}speed", speed);
                    PlayerPrefs.SetInt($"player{playerId}enemiesDefeated", data.player.enemiesDefeated);
                    PlayerPrefs.SetInt($"player{playerId}victories", data.player.victories);

                    PlayerPrefs.Save();

                    Debug.Log($"✅ Valors guardats en PlayerPrefs para jugador {playerId}:\n" +
                        $"Bombas: {PlayerPrefs.GetInt($"player{playerId}Bombs")} \n" +
                        $"Bombas usadas: {PlayerPrefs.GetInt($"player{playerId}BombsUsed")}\n" +
                        $"Velocidad: {PlayerPrefs.GetInt($"player{playerId}speed")}\n" +
                        $"Enemigos derrotados: {PlayerPrefs.GetInt($"player{playerId}enemiesDefeated")} \n" +
                        $"Victorias: {PlayerPrefs.GetInt($"player{playerId}victories")}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"❌ Error al processar JSON: {e.Message}\nJSON rebut: {responseData}\nStack trace: {e.StackTrace}");
                }
            }
            else
            {
                Debug.LogError($"❌ Error al carregar dades: {www.error}\nCodi: {www.responseCode}\nText: {www.downloadHandler?.text}");
            }
        }
    }

    public void Jugar()
    {
        if (AreBothPlayersLoggedIn())
        {
            Debug.Log("Als dos jugadors han iniciat correctament sessió. Carregant escena del joc...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.LogWarning("No tots els jugadors han iniciat sessió.");
        }
    }

    private bool AreBothPlayersLoggedIn()
    {
        return isPlayer1LoggedIn && isPlayer2LoggedIn;
    }
      public void UpdatePlayerStats(int playerNumber, int bombAmount, int bombsUsed, int speed, int victories, int enemiesDefeated)
    {
        List<PlayerInfo> players = new List<PlayerInfo>();

        PlayerInfo player = new PlayerInfo
        {
            username = PlayerPrefs.GetString($"player{playerNumber}Username", ""),
            bombs = bombAmount, 
            bombsUsed = bombsUsed,
            speed = speed,
            victories = victories,
            enemiesDefeated = enemiesDefeated
        };

        if (!string.IsNullOrEmpty(player.username))
        {
            players.Add(player);
            StartCoroutine(SendStatsToServer(players));  
        }
        else
        {
            Debug.LogError($"Usuari no trobat per el jugador {playerNumber}");
        }
    }
    
   public IEnumerator SendStatsToServer(List<PlayerInfo> playersInfo)
    {
        if (playersInfo.Count == 0)
        {
            Debug.LogError("No hi ha jugadors per enviar stats");
            yield break;
        }

        List<PlayerUpdateInfo> players = new List<PlayerUpdateInfo>();
        foreach (var info in playersInfo)
        {
            int playerNumber = 1;
            if (info.username == PlayerPrefs.GetString("player2Username"))
            {
                playerNumber = 2;
            }

            int playerId = PlayerPrefs.GetInt($"player{playerNumber}Id", 0);
            Debug.Log($"Jugador {playerNumber} (ID: {playerId}): {info.username}");

            if (playerId == 0)
            {
                Debug.LogError($"Error: ID no válid pel jugador {playerNumber}");
                continue;
            }

            PlayerUpdateInfo playerUpdate = new PlayerUpdateInfo
            {
                id = playerId,
                username = info.username,
                bombAmount = info.bombs,
                bombsUsed = info.bombsUsed,
                speed = info.speed,
                victories = info.victories,
                enemiesDefeated = info.enemiesDefeated
            };
            players.Add(playerUpdate);
        }

        if (players.Count == 0)
        {
            Debug.LogError("No hi ha jugadoors per actualitzar");
            yield break;
        }

        PlayersData playersData = new PlayersData { players = players };
        string jsonData = JsonUtility.ToJson(playersData);
        Debug.Log($"Enviant dades al servidor:\n{jsonData}");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(updateStatsURL, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            string responseText = www.downloadHandler?.text ?? "No response";
          

            if (www.result == UnityWebRequest.Result.Success && !string.IsNullOrEmpty(responseText))
            {
                Debug.Log($"📡 Stats enviades correctament per {players.Count} jugadores.");
            }
            else
            {
                string error = www.error;
                if (string.IsNullOrEmpty(error) && www.responseCode != 200)
                {
                    error = $"HTTP Error {www.responseCode}";
                }
                Debug.LogError($"Error al actualizar stats:\n" +
                    $"URL: {updateStatsURL}\n" +
                    $"Error: {error}\n" +
                    $"Codi HTTP: {www.responseCode}\n" +
                    $"Resposta: {responseText}\n" +
                    $"JSON enviat: {jsonData}");
            }
        }
    }
}
