using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button playButton = root.Q<Button>("playButton");
        Button optionsButton = root.Q<Button>("optionsButton");
        Button exitButton = root.Q<Button>("exitButton");

        playButton.clicked += () => SceneManager.LoadScene("Bomberman");
        exitButton.clicked += ExitGame;
    }

   private void ExitGame()
    {
        Debug.Log("Sutint del joc...");
        Application.Quit(); 

        
    }
}