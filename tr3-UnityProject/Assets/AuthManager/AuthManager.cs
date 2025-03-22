using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;


public class AuthManager : MonoBehaviour
{
    [Header("Login")]
    public TMP_InputField usernameLoginInput;
    public TMP_InputField passwordLoginInput;

    [Header("Register")]
    public TMP_InputField usernameRegisterInput;
    public TMP_InputField emailRegisterInput;
    public TMP_InputField passwordRegisterInput;
    public TextMeshProUGUI messageText;
    private string apiUrl = "http://localhost:3020/players"; 

    public void Register()
    {
        Debug.Log("Intentando registrar: " + usernameRegisterInput.text + ", " + emailRegisterInput.text + ", " + passwordRegisterInput.text);
        StartCoroutine(RegisterCoroutine());
    }

    public void Login()
    {
        Debug.Log("Intentando iniciar sesión: " + usernameRegisterInput.text + ", " + passwordRegisterInput.text);
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator RegisterCoroutine()
{
    Debug.Log("usernameInput: " + usernameRegisterInput);
    Debug.Log("emailInput: " + emailRegisterInput);
    Debug.Log("passwordInput: " + passwordRegisterInput);
    
    if (usernameRegisterInput == null || emailRegisterInput == null || passwordRegisterInput == null)
    {
        Debug.LogError("Uno o más campos no están asignados en el Inspector de Unity.");
        yield break;
    }

    string url = apiUrl + "/register";
    WWWForm form = new WWWForm();
    form.AddField("username", usernameRegisterInput.text);
    form.AddField("email", emailRegisterInput.text);
    form.AddField("password", passwordRegisterInput.text);

    UnityWebRequest request = UnityWebRequest.Post(url, form);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        messageText.text = "Registro exitoso!";
    }
    else
    {
        messageText.text = "Error: " + request.downloadHandler.text;
    }
}

private IEnumerator LoginCoroutine()
{
    string username = usernameLoginInput.text;
    string password = passwordLoginInput.text;

    Debug.Log("Intentando iniciar sesión con username: " + username + " y password: " + password);  // Asegúrate de ver los valores en consola

    string url = apiUrl + "/login";
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("password", password);

    UnityWebRequest request = UnityWebRequest.Post(url, form);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        string json = request.downloadHandler.text;
        Debug.Log("Respuesta del servidor: " + json);

        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        PlayerPrefs.SetString("token", data.token);
        PlayerPrefs.SetInt("playerId", data.player.id);
        messageText.text = "Login exitoso!";

        SceneManager.LoadScene("Bomberman");
    }
    else
    {
        Debug.LogError("Login failed: " + request.error);  // Muestra el error en la consola de Unity
        messageText.text = "Error: " + request.downloadHandler.text;
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
