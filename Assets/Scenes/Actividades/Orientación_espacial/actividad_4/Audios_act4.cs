using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Actividad4Manager : MonoBehaviour
{
    public static Actividad4Manager Instance;

    [Header("Audios")]
    public AudioSource audioSource;
    public AudioClip audioGeneral;
    public AudioClip audioSol;
    public AudioClip audioFlor;
    public AudioClip audioArbol;
    public AudioClip audioVaca;

    [Header("Referencias")]
    public GameObject sol;
    public GameObject flor;
    public GameObject arbol;
    public GameObject vaca;
    public Button botonGeneral; // CAMBIA: es Button ahora, no GameObject

    [Header("Panel Confirmación")]
    public GameObject panelConfirmacion;
    public Button botonContinuar; // círculo
    public Button botonRepetir;   // cuadrado

    private Dictionary<GameObject, AudioClip> audioPorObjeto;
    private Dictionary<GameObject, bool> objetosMovidos;

    private float tiempoUltimoMovimiento = -1f;
    private float tiempoInactividad = 0f;
    private bool esperandoConfirmacion = false;
    private bool panelYaMostrado = false;
    private bool estaArrastrando = false; // NUEVO

    void Awake()
    {
        Instance = this;
        panelConfirmacion.SetActive(false);
    }

    void Start()
    {
        // Mapear objetos con sus sonidos
        audioPorObjeto = new Dictionary<GameObject, AudioClip>()
        {
            { sol, audioSol },
            { flor, audioFlor },
            { arbol, audioArbol },
            { vaca, audioVaca }
        };

        objetosMovidos = new Dictionary<GameObject, bool>()
        {
            { sol, false },
            { flor, false },
            { arbol, false },
            { vaca, false }
        };

        botonContinuar.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Actividad5");
        });

        botonRepetir.onClick.AddListener(() =>
        {
            panelConfirmacion.SetActive(false);
            esperandoConfirmacion = false;
            tiempoUltimoMovimiento = -1f;
            tiempoInactividad = 0f;
            panelYaMostrado = true;
        });

        botonGeneral.onClick.AddListener(() => ReproducirAudioGeneral()); // <<< NUEVO
    }

    void ReproducirAudioGeneral()
    {
        audioSource.clip = audioGeneral;
        audioSource.Play();
    }

    public void ObjetoTocado(GameObject objeto)
    {
        if (audioPorObjeto.ContainsKey(objeto))
        {
            audioSource.clip = audioPorObjeto[objeto];
            audioSource.Play();
        }
    }

    public void ObjetoMovido(GameObject objeto)
    {
        if (objetosMovidos.ContainsKey(objeto) && !objetosMovidos[objeto])
        {
            objetosMovidos[objeto] = true;
        }

        tiempoUltimoMovimiento = Time.time;
        tiempoInactividad = 0f;
        estaArrastrando = false;

        if (TodosObjetosMovidos() && !panelYaMostrado)
        {
            esperandoConfirmacion = true;
            Invoke(nameof(MostrarPanelConfirmacion), 3f);
        }
    }

    bool TodosObjetosMovidos()
    {
        foreach (var movido in objetosMovidos.Values)
        {
            if (!movido) return false;
        }
        return true;
    }

    void MostrarPanelConfirmacion()
    {
        if (!esperandoConfirmacion) return;

        panelConfirmacion.SetActive(true);
    }

    void Update()
    {
        if (!panelYaMostrado) return;

        if (estaArrastrando) return; // NO contar inactividad mientras se arrastra

        tiempoInactividad += Time.deltaTime;

        if (!esperandoConfirmacion)
        {
            if (tiempoUltimoMovimiento < 0 && tiempoInactividad >= 5f)
            {
                esperandoConfirmacion = true;
                MostrarPanelConfirmacion();
            }

            if (tiempoUltimoMovimiento > 0 && Time.time - tiempoUltimoMovimiento >= 3f)
            {
                esperandoConfirmacion = true;
                MostrarPanelConfirmacion();
            }
        }
    }

    public void EmpezarArrastre()
    {
        estaArrastrando = true;
    }
}
