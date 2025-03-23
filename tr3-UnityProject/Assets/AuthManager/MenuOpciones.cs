using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void VolumMusica(float volumen)
    {
        Debug.Log("Volumen de la música: " + volumen);
        audioMixer.SetFloat("VolumMusica", volumen);
    }
    public void QualitatPantalla(int qualitat)
    {
        Debug.Log("Qualitat de la pantalla: " + qualitat);
        QualitySettings.SetQualityLevel(qualitat);
    }
}
