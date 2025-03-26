using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using MyGame.Models;

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
                PlayerPrefs.SetInt("player1Bombs", data.player.bombs);
                PlayerPrefs.SetInt("player1Victories", data.player.victories);
                PlayerPrefs.SetInt("player1EnemiesDefeated", data.player.enemiesDefeated);
                PlayerPrefs.Save();

                messageText.text = "Jugador 1 conectado correctamente!";
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
            Debug.LogError($"Error de conexión Jugador 1: {request.error}");
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
                PlayerPrefs.SetInt("player2Bombs", data.player.bombs);
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
        Debug.Log("Fetching stats from: " + url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string responseData = www.downloadHandler.text;
                Debug.Log("Response data: " + responseData);

                PlayerData data = JsonUtility.FromJson<PlayerData>(responseData);
                PlayerPrefs.SetInt($"player{playerId}bombs", data.player.bombs);
                PlayerPrefs.SetInt($"player{playerId}speed", data.player.speed);
                PlayerPrefs.SetInt($"player{playerId}enemiesDefeated", data.player.enemiesDefeated);
                PlayerPrefs.SetInt($"player{playerId}wins", data.player.victories);

                PlayerPrefs.Save();
                Debug.Log($"Stats carreguedes pel jugador {playerId}: bombs={data.player.bombs}, speed={data.player.speed}, enemiesDefeated={data.player.enemiesDefeated}, wins={data.player.victories}");
            }
            else
            {
                Debug.LogError("Error carregant dades: " + www.error);
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
}
