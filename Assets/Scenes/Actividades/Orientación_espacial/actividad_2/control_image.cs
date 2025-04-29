using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// 🔵 CAMBIAMOS el nombre de la clase
public class Control_image : MonoBehaviour
{
    private Camb_Image imagenSeleccionada;
    public GameObject panelConfirmacion;

    private Coroutine esperaConfirmacion;

    private string nombreActividad = "Actividad2"; // ya estaba correcto
    private string resultado = "Competente"; // ya estaba correcto

    public void Seleccionar(Camb_Image nuevaSeleccion)
    {
        if (imagenSeleccionada != null && imagenSeleccionada != nuevaSeleccion)
        {
            imagenSeleccionada.Deseleccionar();
        }

        imagenSeleccionada = nuevaSeleccion;

        if (esperaConfirmacion != null)
        {
            StopCoroutine(esperaConfirmacion);
        }
        esperaConfirmacion = StartCoroutine(EsperarYMostrarPanel());
    }

    private IEnumerator EsperarYMostrarPanel()
    {
        yield return new WaitForSeconds(2f);
        panelConfirmacion.SetActive(true);
    }

    public void ReiniciarSeleccion()
    {
        panelConfirmacion.SetActive(false);
        imagenSeleccionada?.Deseleccionar();
        imagenSeleccionada = null;
    }

    public void IrASiguienteActividad()
    {
        GuardarResultado();
        Debug.Log("Actividad3");
        SceneManager.LoadScene("Actividad3");
    }

    private void GuardarResultado()
    {
        if (ResultadosManager.instancia != null)
        {
            ResultadosManager.instancia.GuardarResultado(nombreActividad, resultado);
            Debug.Log("Resultado de " + nombreActividad + " guardado: " + resultado);
        }
        else
        {
            Debug.LogWarning("No se encontró ResultadosManager");
        }
    }
}
