using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class DatosSesion_A1 : MonoBehaviour
{
    private CambiarImagen imagenSeleccionada;
    public GameObject panelConfirmacion;
    private Coroutine esperaConfirmacion;

    private string nombreActividad = "Actividad2";

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
        Debug.Log("➡️ Pasando a siguiente actividad...");
        StartCoroutine(GuardarYContinuar());
    }

    private IEnumerator GuardarYContinuar()
    {
        float duracion = Time.timeSinceLevelLoad;

        string respuestaSeleccionada = imagenSeleccionada.opcionNombre; // <-- por ejemplo: "delante"
        bool esCorrecta = imagenSeleccionada.esCorrecta; // <-- depende del botón tocado

        ResultadoActividades resultadoData = new ResultadoActividades(nombreActividad)
        {
            respuestaSeleccionada = respuestaSeleccionada,
            esCorrecta = esCorrecta,
            duracionSegundos = duracion,
            vecesEscuchoInstruccion = 1,
            timestamp = System.DateTime.UtcNow.ToString("o"),
            imagenBase64 = ""
        };

        yield return StartCoroutine(EnviarResultado(resultadoData));
        SceneManager.LoadScene("Actividad2");
    }

    private IEnumerator EnviarResultado(ResultadoActividades resultadoData)
    {
        string jsonData = JsonUtility.ToJson(resultadoData);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest("https://biuno-server.onrender.com/data", "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Datos enviados correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error al enviar datos: " + request.error);
        }
    }
}
