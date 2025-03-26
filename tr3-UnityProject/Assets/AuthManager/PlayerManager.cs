// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using System.Text;
// using SimpleJSON;

// public class PlayerManager : MonoBehaviour
// {
//     private string baseUrl = "http://localhost:3020/players"; 

//     public static PlayerManager Instance;
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//     }
//     // private void Start()
//     // {
//     //     StartCoroutine(LoadStatsFromServer("http://localhost:4000/api/personatges/1"));
//     // }

//     // private IEnumerator LoadStatsFromServer(string url)
//     // {
//     //     using (UnityWebRequest www = UnityWebRequest.Get(url))
//     //     {
//     //         yield return www.SendWebRequest();

//     //         if (www.result == UnityWebRequest.Result.Success)
//     //         {
//     //             PlayerStats stats = JsonUtility.FromJson<PlayerStats>(www.downloadHandler.text);
//     //             moveSpeed = stats.moveSpeed;
//     //             coinMultiplier = stats.coinMultiplier;
//     //             Debug.Log($"Stats cargados desde el servidor: speed={moveSpeed}, coinX{coinMultiplier}");
//     //         }
//     //         else
//     //         {
//     //             Debug.LogError("Error cargando datos: " + www.error);
//     //         }
//     //     }
//     // }

//     public void GetPlayerData(int playerId)
//     {
//         StartCoroutine(FetchPlayerData(playerId));
//     }

//     private IEnumerator FetchPlayerData(int playerId)
//     {
//         string url = $"{baseUrl}/{playerId}";
//         UnityWebRequest request = UnityWebRequest.Get(url);

//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             string responseData = request.downloadHandler.text;
//             var json = JSON.Parse(responseData);

//             PlayerPrefs.SetString("username", json["username"]);
//             PlayerPrefs.SetInt("bombs", json["bombs"].AsInt);
//             PlayerPrefs.SetInt("victories", json["victories"].AsInt);
//             PlayerPrefs.SetInt("enemiesDefeated", json["enemiesDefeated"].AsInt);

//             Debug.Log("Datos del jugador cargados: " + json["username"]);
//         }
//         else
//         {
//             Debug.LogError("Error al obtener datos del jugador: " + request.error);
//         }
//     }

//     public void SetPlayerData(int playerNumber, PlayerInfo playerInfo)
//     {
//         StartCoroutine(UpdatePlayerData(playerNumber, playerInfo));
//     }

//     private IEnumerator UpdatePlayerData(int playerNumber, PlayerInfo playerInfo)
//     {
//         string url = baseUrl + "/" + playerInfo.id;
//         string json = JsonUtility.ToJson(playerInfo);

//         UnityWebRequest request = new UnityWebRequest(url, "PUT");
//         byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             Debug.Log("Datos del jugador actualizados.");
//         }
//         else
//         {
//             Debug.LogError("Error al actualizar datos del jugador: " + request.error);
//         }
//     }
// }
