using UnityEngine;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public static VictoryScreen instance;

    public GameObject victoryCanvas;  
    public TextMeshProUGUI winnerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        victoryCanvas.SetActive(false);  
    }

    public void ShowVictoryScreen(string winnerName)
    {
        victoryCanvas.SetActive(true);
        winnerText.text = $"Guanyador: {winnerName}!";
    }
}
