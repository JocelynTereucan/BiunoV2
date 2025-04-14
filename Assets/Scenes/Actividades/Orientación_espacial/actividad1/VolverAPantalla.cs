using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAPantallaInstruccion : MonoBehaviour
{
    public string nombreEscenaInstruccion = "inicio"; // cámbialo por el nombre real de tu escena

    public void Volver()
    {
        SceneManager.LoadScene(nombreEscenaInstruccion);
    }
}
