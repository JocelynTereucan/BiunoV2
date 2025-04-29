using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecuperarContrasena : MonoBehaviour
{
    public InputField inputCorreo;
    public Text mensajeTexto;
    public GestorPantallas gestorPantallas; // Referencia al gestor de pantallas

    public void EnviarSolicitud()
    {
        string correoIngresado = inputCorreo.text;

        if (string.IsNullOrEmpty(correoIngresado))
        {
            mensajeTexto.text = "Por favor, ingresa tu correo.";
            return;
        }

        // Aquí podrías conectar con un backend para verificar el correo
        if (EsCorreoValido(correoIngresado))
        {
            mensajeTexto.text = "Se ha enviado un enlace de recuperación a tu correo.";
        }
        else
        {
            mensajeTexto.text = "El correo ingresado no está registrado.";
        }
    }

    private bool EsCorreoValido(string correo)
    {
        // Simulación de verificación (esto se haría con una base de datos en un sistema real)
        return correo.Contains("@") && correo.Contains(".");
    }

    public void RegresarAlInicio()
    {
        gestorPantallas.CambiarPantalla("InicioSesion");
    }
}
