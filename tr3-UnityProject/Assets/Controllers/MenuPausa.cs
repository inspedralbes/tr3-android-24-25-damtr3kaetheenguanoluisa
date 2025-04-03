using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;
    private bool jocPausa = false;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(jocPausa){
                Reanudar();
            }else{
                Pausa();
            }
        }
    }
    public void Pausa()
    {
        jocPausa = true;
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }
    public void Reanudar(){
        jocPausa = false;
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }
    public void Reiniciar(){
        jocPausa = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Cerrar(){
        Debug.Log("Tancant joc...");
        Application.Quit();
    }
    public void Home(){
        SceneManager.LoadScene("Main");
    }
}
