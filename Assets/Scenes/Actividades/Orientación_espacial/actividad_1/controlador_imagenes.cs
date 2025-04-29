using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControladorImagenes : MonoBehaviour
{
    private CambiarImagen imagenSeleccionada;
    public GameObject panelConfirmacion; // <<< nuevo
    private Coroutine esperaConfirmacion; // <<< nuevo

    private string nombreActividad = "Actividad1"; // <<< agregado
    private string resultado = "Competente"; // <<< agregado por defecto

    public void Seleccionar(CambiarImagen nuevaSeleccion)
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
        GuardarResultado(); // <<< agregado antes de cambiar de escena
        Debug.Log("Actividad2");
        SceneManager.LoadScene("Actividad2");
    }

    private void GuardarResultado() // <<< NUEVO MÉTODO
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
