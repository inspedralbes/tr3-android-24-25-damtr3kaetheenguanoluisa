using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
   public void Jugar (){
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void Salir()
    {
        Debug.Log("Surtint del joc...");
        Application.Quit(); 
    }
}