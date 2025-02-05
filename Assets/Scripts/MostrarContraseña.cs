using UnityEngine;
using UnityEngine.UI;
using TMPro; // Necesario para TMP_InputField

public class MostrarContraseña : MonoBehaviour
{
    public TMP_InputField inputField;
    public Image ojoImagen;
    public Sprite ojoAbierto;
    public Sprite ojoCerrado;
    
    private bool mostrando = false;

    public void AlternarContraseña()
    {
        mostrando = !mostrando;
        inputField.contentType = mostrando ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        inputField.ForceLabelUpdate();
        ojoImagen.sprite = mostrando ? ojoAbierto : ojoCerrado;
    }
}
