using UnityEngine;
using UnityEngine.SceneManagement; // Para cambiar de escena en el futuro

public class BotonOlvidasteContraseña : MonoBehaviour
{
    public void AlHacerClick()
    {
        Debug.Log("Aquí irá la pantalla de cambio de contraseña"); // Mensaje temporal
        // SceneManager.LoadScene("PantallaCambioContraseña"); // Activa esto cuando tengas la escena creada
    }
}

public class GestorPantallas : MonoBehaviour
{
    public void CambiarPantalla(string nombrePantalla)
    {
        SceneManager.LoadScene(nombrePantalla);
    }
}
