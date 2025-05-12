using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ControladorImagenes : MonoBehaviour
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

        Actividad1Resultado resultadoData = new Actividad1Resultado
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

        Debug.Log("➡️ Cambio de escena tras envío exitoso o fallo");
        SceneManager.LoadScene("Actividad2");
    }


    private void GuardarResultado()
    {
        Debug.Log("🧠 GuardarResultado() fue llamado");

        float duracion = Time.time - tiempoInicio;

        Actividad1Resultado resultadoData = new Actividad1Resultado
        {
            childId = "child_001",
            respuestaSeleccionada = respuestaSeleccionada,
            esCorrecta = (respuestaSeleccionada == "abajo"),
            duracionSegundos = duracion,
            vecesEscuchoInstruccion = cantidadReproducciones,
            timestamp = System.DateTime.UtcNow.ToString("o"),
            imagenBase64 = "" // La imagen se omitirá por ahora
        };

        Debug.Log("📦 Resultado creado, preparando envío sin imagen...");
        // StartCoroutine(CapturarYEnviarResultado(resultadoData)); // Comentado por ahora
        StartCoroutine(EnviarResultado(resultadoData)); // Envío directo sin imagen
    }

    /*
    // Este método queda comentado temporalmente
    private IEnumerator CapturarYEnviarResultado(Actividad1Resultado resultadoData)
    {
        yield return new WaitForEndOfFrame();

        Debug.Log("📸 Capturando pantalla...");
        Texture2D screenImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        byte[] imageBytes = screenImage.EncodeToPNG();
        string imagenBase64 = System.Convert.ToBase64String(imageBytes);
        resultadoData.imagenBase64 = imagenBase64;

        Destroy(screenImage);

        Debug.Log("🚀 Preparando envío al servidor...");
        StartCoroutine(EnviarResultado(resultadoData));
    }
    */

    private IEnumerator EnviarResultado(Actividad1Resultado resultadoData)
    {
        string jsonData = JsonUtility.ToJson(resultadoData);
        Debug.Log("📤 Enviando JSON al servidor: " + jsonData.Substring(0, Mathf.Min(300, jsonData.Length)));

        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(urlServidor, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("🌐 Enviando request a: " + urlServidor);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Respuesta recibida del servidor: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error al enviar datos: " + request.error);
            Debug.LogError("🧾 Código de respuesta: " + request.responseCode);
            Debug.LogError("📝 Mensaje del servidor (si hay): " + request.downloadHandler.text);
        }
    }
}
