using UnityEngine;
using UnityEngine.UI;

public class CambiarImagen : MonoBehaviour
{
    public Sprite imagenNormal;
    public Sprite imagenTachada;
    public ControladorImagenes controlador;

    private Image imagenActual;
    private bool estaTachado = false;

    private void Start()
    {
        imagenActual = GetComponent<Image>();
    }

    public void Cambiar()
    {
        if (!estaTachado)
        {
            controlador.Seleccionar(this);
            imagenActual.sprite = imagenTachada;
            estaTachado = true;
        }
    }

    public void Deseleccionar()
    {
        imagenActual.sprite = imagenNormal;
        estaTachado = false;
    }
}
