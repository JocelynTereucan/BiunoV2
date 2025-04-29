using UnityEngine;
using UnityEngine.SceneManagement; // Para cambiar de escena en el futuro


public class GestorPantallas : MonoBehaviour
{
    public void CambiarPantalla(string nombrePantalla)
    {
        SceneManager.LoadScene(nombrePantalla);
        
    }

    
}


