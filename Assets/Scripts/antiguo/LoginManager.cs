using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField input_Correo;
    public TMP_InputField input_Contraseña;
    public GameObject Panel; // Panel de error
    public TMP_Text mensajeError; // Mensaje dentro del panel
    public GestorPantallas GestorPantallas; // Gestor de cambio de pantalla

    private string correoCorrecto = "educadora@test.com";
    private string contraseñaCorrecta = "1234";

    public void IniciarSesion()
    {
        string correoIngresado = input_Correo.text.Trim(); // Evitar espacios vacíos
        string contraseñaIngresada = input_Contraseña.text.Trim();

        if (correoIngresado == correoCorrecto && contraseñaIngresada == contraseñaCorrecta)
        {
            GestorPantallas.CambiarPantalla("InicioEducadora"); // Cambia a la pantalla principal
        }
        else
        {
            MostrarError("Correo o contraseña incorrectos.");
        }
    }

    public void OlvidasteContrasena()
    {
        GestorPantallas.CambiarPantalla("CambioContrasena");
    }

    void MostrarError(string mensaje)
    {
        if (Panel != null && mensajeError != null)
        {
            mensajeError.text = mensaje;
            Panel.SetActive(true);
        }
        else
        {
            Debug.LogError("Panel o mensajeError no están asignados en Unity.");
        }
    }

    public void CerrarError()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }
}
