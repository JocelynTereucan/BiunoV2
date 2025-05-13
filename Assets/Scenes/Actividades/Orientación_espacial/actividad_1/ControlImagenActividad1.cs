using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class ControlImagenActividad1 : MonoBehaviour
{
    private CambiarImagen imagenSeleccionada;
    public GameObject panelConfirmacion;

    private Coroutine esperaConfirmacion;

    private string resultado = "Competente";
    private float tiempoInicio;
    private int cantidadReproducciones = 0;
    private string respuestaSeleccionada = "";

    private string urlServidor = "https://biuno-server.onrender.com/data";

    void Start()
    {
        tiempoInicio = Time.time;
        Debug.Log("🟢 Unity iniciado - URL del servidor: " + urlServidor);
    }

    public void Seleccionar(CambiarImagen nuevaSeleccion)
    {
        if (imagenSeleccionada != null && imagenSeleccionada != nuevaSeleccion)
        {
            imagenSeleccionada.Deseleccionar();
        }

        imagenSeleccionada = nuevaSeleccion;

        float ySeleccionada = nuevaSeleccion.transform.localPosition.y;
        respuestaSeleccionada = ySeleccionada > 0 ? "arriba" : "abajo";
        resultado = (respuestaSeleccionada == "abajo") ? "Competente" : "Deficiente";

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

    public void ReproducirInstruccion()
    {
        cantidadReproducciones++;
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
        float duracion = Time.time - tiempoInicio;

        ResultadoActividades resultadoData = new ResultadoActividades("Actividad 1")
        {
            childId = "child_001",
            respuestaSeleccionada = respuestaSeleccionada,
            esCorrecta = (respuestaSeleccionada == "abajo"),
            duracionSegundos = duracion,
            vecesEscuchoInstruccion = cantidadReproducciones,
            timestamp = System.DateTime.UtcNow.ToString("o"),
            imagenBase64 = ""
        };

        Debug.Log("📦 Resultado creado, enviando antes de cambiar de escena...");
        yield return StartCoroutine(EnviarResultado(resultadoData));

        SceneManager.LoadScene("Actividad2");
    }

    private IEnumerator EnviarResultado(ResultadoActividades resultadoData)
    {
        string jsonData = JsonUtility.ToJson(resultadoData);
        Debug.Log("📤 Enviando JSON al servidor: " + jsonData);

        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(urlServidor, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Datos enviados exitosamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error al enviar datos: " + request.error);
        }
    }
}
