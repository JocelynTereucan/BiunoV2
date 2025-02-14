using TMPro;
using UnityEngine;

public class InputFieldHandler : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    private string defaultEmail = "Educador@example.com";
    private string defaultPassword = "Contraseña";

    private void Start()
    {
        // Asegurar que los campos tengan el placeholder correcto al inicio
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            emailInputField.text = defaultEmail;
        }
        if (string.IsNullOrEmpty(passwordInputField.text))
        {
            passwordInputField.text = defaultPassword;
            passwordInputField.contentType = TMP_InputField.ContentType.Standard; // Mostrar texto normal
            passwordInputField.ForceLabelUpdate();
        }
    }

    public void ClearEmailText()
    {
        if (emailInputField.text == defaultEmail)
        {
            emailInputField.text = "";
        }
    }

    public void ClearPasswordText()
{
    if (passwordInputField.text == defaultPassword)
    {
        passwordInputField.text = "";
        passwordInputField.contentType = TMP_InputField.ContentType.Password; // Convertir a contraseña
        passwordInputField.ForceLabelUpdate();
    }
}


    public void RestoreEmailPlaceholder()
    {
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            emailInputField.text = defaultEmail;
        }
    }

    public void RestorePasswordPlaceholder()
    {
        if (string.IsNullOrEmpty(passwordInputField.text))
        {
            passwordInputField.text = defaultPassword;
            passwordInputField.contentType = TMP_InputField.ContentType.Standard; // Mostrar texto normal
            passwordInputField.ForceLabelUpdate();
        }
    }
}
