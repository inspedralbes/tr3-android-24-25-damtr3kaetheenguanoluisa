using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Text;

public class PlayerManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:3000/api/player";

    public void GetPlayerData(int playerId)
    {
        StartCoroutine(FetchPlayerData(playerId));
    }

    public void UpdatePlayerData(int playerId, string username, int bombs, int victories, int enemiesDefeated)
    {
        StartCoroutine(UpdatePlayer(playerId, username, bombs, victories, enemiesDefeated));
    }

    private IEnumerator FetchPlayerData(int playerId)
    {
        string token = PlayerPrefs.GetString("token", "");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No hay token guardado.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/" + playerId);
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseData = request.downloadHandler.text;
            Debug.Log("Datos del jugador recibidos: " + responseData);
        }
        else
        {
            Debug.LogError("Error al obtener datos del jugador: " + request.error);
        }
    }

    private IEnumerator UpdatePlayer(int playerId, string username, int bombs, int victories, int enemiesDefeated)
    {
        string token = PlayerPrefs.GetString("token", "");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No hay token guardado.");
            yield break;
        }

        string json = $"{{\"username\":\"{username}\",\"bombs\":{bombs},\"victories\":{victories},\"enemiesDefeated\":{enemiesDefeated}}}";

        UnityWebRequest request = new UnityWebRequest(baseUrl + "/" + playerId, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos del jugador actualizados correctamente.");
        }
        else
        {
            Debug.LogError("Error al actualizar jugador: " + request.error);
        }
    }
}
