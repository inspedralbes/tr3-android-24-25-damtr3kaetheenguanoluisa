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
        StartCoroutine(LoginCoroutinePlayer1(usernameLoginInputP1.text, passwordLoginInputP1.text, 1));
    }

    public void LoginPlayer2()
    {
        Debug.Log("Iniciant sessió jugador2...");
        StartCoroutine(LoginCoroutinePlayer2(usernameLoginInputP2.text, passwordLoginInputP2.text, 2));
    }

    private IEnumerator RegisterCoroutine(string username, string email, string password, int playerNumber)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError($"Falten dades a registrar del jugador {playerNumber}");
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
            Debug.Log($"Registre omplert del jugador {playerNumber}!");
        }
        else
        {
            Debug.LogError($"Error registrat Player {playerNumber}: {request.downloadHandler.text}");
        }
    }

    private IEnumerator LoginCoroutinePlayer1(string username, string password, int playerNumber)
{

    string url = apiUrl + "/login";
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("password", password);

    UnityWebRequest request = UnityWebRequest.Post(url, form);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        isPlayer1LoggedIn = true;
        messageText.text = "Login jugador 1!";

        PlayerData data = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
        PlayerDataManager.Instance.SetPlayerData(1, data.player); 

    }
    else
    {
        messageText.text = "Error al iniciar sesión jugador 1: " + request.downloadHandler.text;
    }
}

    private IEnumerator LoginCoroutinePlayer2(string username, string password, int playerNumber)
    {
        string url = apiUrl + "/login";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            isPlayer2LoggedIn = true;
            Debug.Log("Login jugador 2!");

            PlayerData data = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
            PlayerDataManager.Instance.SetPlayerData(2, data.player); 

        }
        else
        {
            messageText.text = "Error al iniciar sessió jugador 2: " + request.downloadHandler.text;
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
