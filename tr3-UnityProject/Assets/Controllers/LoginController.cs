using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using Newtonsoft.Json;

public class LoginController : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField userNameInput; 
    public TextMeshProUGUI messageText; 

    private string baseUrl = "http://localhost:3020/players";

    public void LoginUser()
    {
        StartCoroutine(LoginCoroutine(emailInput.text, passwordInput.text));
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        string json = JsonUtility.ToJson(new { email, password });
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Manejo de errores
        if (request.result == UnityWebRequest.Result.Success)
        {
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(request.downloadHandler.text);
            PlayerPrefs.SetString("UserEmail", user.email);
            PlayerPrefs.SetString("UserName", user.userName);
            messageText.text = "Login exitoso!";
            Debug.Log("Usuario logueado: " + user.userName);
        }
        else
        {
            messageText.text = "Error: " + request.downloadHandler.text;
            Debug.LogError("Error en Login: " + request.downloadHandler.text);
        }
    }

    public void RegisterUser()
    {
        StartCoroutine(RegisterCoroutine(userNameInput.text, emailInput.text, passwordInput.text));
    }

    IEnumerator RegisterCoroutine(string userName, string email, string password)
    {
        string json = JsonUtility.ToJson(new { userName, email, password });
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/register", "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            messageText.text = "Registro exitoso!";
            Debug.Log("Usuario registrado: " + email);
        }
        else
        {
            messageText.text = "Error: " + request.downloadHandler.text;
            Debug.LogError("Error en Registro: " + request.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class UserResponse
{
    public string userName;
    public string email;
}