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
        string correoIngresado = input_Correo.text;
        string contraseñaIngresada = input_Contraseña.text;

        if (correoIngresado == correoCorrecto && contraseñaIngresada == contraseñaCorrecta)
        {
            GestorPantallas.CambiarPantalla("InicioEducadora"); // Cambia a la pantalla principal
        }
        else
        {
            MostrarError();
        }
    }

    void MostrarError()
    {
        
        Panel.SetActive(true);
    }

    public void CerrarError()
    {
        Panel.SetActive(false);
    }
}
