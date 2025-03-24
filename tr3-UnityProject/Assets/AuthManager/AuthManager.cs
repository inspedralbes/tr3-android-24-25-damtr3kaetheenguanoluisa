using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Player 1 Login")]
    public TMP_InputField usernameLoginInputP1;
    public TMP_InputField passwordLoginInputP1;

    [Header("Player 2 Login")]
    public TMP_InputField usernameLoginInputP2;
    public TMP_InputField passwordLoginInputP2;

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
        Debug.Log("Registrant jugador1...");
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
        StartCoroutine(LoginCoroutine(usernameLoginInputP1.text, passwordLoginInputP1.text, 1));
    }
     public void LoginPlayer2()
    {
        Debug.Log("Iniciant sessió jugador2...");
        StartCoroutine(LoginCoroutine(usernameLoginInputP2.text, passwordLoginInputP2.text, 2));
    }

    private IEnumerator RegisterCoroutine(string username, string email, string password, int playerNumber)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError($"Faltan datos para registrar al jugador {playerNumber}");
            yield break;
        }

        string url = apiUrl + "/register";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Registro exitoso para Player {playerNumber}!");
        }
        else
        {
            Debug.LogError($"Error registrando Player {playerNumber}: {request.downloadHandler.text}");
        }
    }

    private IEnumerator LoginCoroutine(string username, string password, int playerNumber)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError($"Faltan datos para iniciar sesión del jugador {playerNumber}");
            yield break;
        }

        string url = apiUrl + "/login";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            PlayerPrefs.SetString($"token{playerNumber}", data.token);
            PlayerPrefs.SetInt($"playerId{playerNumber}", data.player.id);

            Debug.Log($"Login exitoso para Player {playerNumber}!");
        }
        else
        {
            Debug.LogError($"Error en login de Player {playerNumber}: {request.downloadHandler.text}");
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public bool success;
    public string message;
    public string token;
    public PlayerInfo player;
}

[System.Serializable]
public class PlayerInfo
{
    public int id;
    public string username;
    public int bombs;
    public int victories;
    public int enemiesDefeated;
}
