using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class ControlImageActividad2 : MonoBehaviour
{
    private CambiarImagenActividad2 imagenSeleccionada;
    public GameObject panelConfirmacion;

    private Coroutine esperaConfirmacion;
    private string nombreActividad = "Actividad 2";

    private string respuestaSeleccionada = "";
    private bool esCorrecta = false;
    private float tiempoInicio;
    private int cantidadReproducciones = 0;

    private string urlServidor = "https://biuno-server.onrender.com/data";

    void Start()
    {
        tiempoInicio = Time.time;
        Debug.Log("🟢 Actividad 2 iniciada - Enviando a: " + urlServidor);
    }

    public void Seleccionar(CambiarImagenActividad2 nuevaSeleccion)
    {
        if (imagenSeleccionada != null && imagenSeleccionada != nuevaSeleccion)
        {
            imagenSeleccionada.Deseleccionar();
        }

        imagenSeleccionada = nuevaSeleccion;
        respuestaSeleccionada = nuevaSeleccion.opcionNombre;
        esCorrecta = nuevaSeleccion.esCorrecta;

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
        Debug.Log("➡️ Siguiente actividad desde A2...");
        StartCoroutine(GuardarYContinuar());
    }

    private IEnumerator GuardarYContinuar()
    {
        float duracion = Time.time - tiempoInicio;

        ResultadoActividades resultadoData = new ResultadoActividades(nombreActividad)
        {
            childId = "child_001", // Puedes hacerlo dinámico si tienes un sistema de login
            respuestaSeleccionada = respuestaSeleccionada,
            esCorrecta = esCorrecta,
            duracionSegundos = duracion,
            vecesEscuchoInstruccion = cantidadReproducciones,
            timestamp = System.DateTime.UtcNow.ToString("o"),
            imagenBase64 = "" // Agrega si capturas imagen
        };

        Debug.Log("📤 Enviando datos desde A2...");
        yield return StartCoroutine(EnviarResultado(resultadoData));

        SceneManager.LoadScene("Actividad3"); // o la que siga
    }

    private IEnumerator EnviarResultado(ResultadoActividades resultadoData)
    {
        string jsonData = JsonUtility.ToJson(resultadoData);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(urlServidor, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Resultado A2 enviado correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error al enviar desde A2: " + request.error);
        }
    }
}
