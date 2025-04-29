using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ArrastrarPato : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public DatabaseManager dbManager;
    private RectTransform rectTransform;
    private Canvas canvas;
    public GameObject panelListo;
    public AudioSource audioListo;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (panelListo != null)
        {
            panelListo.SetActive(true);
        }

        if (audioListo != null)
        {
            audioListo.Play();
        }
    }

    public void ConfirmarEvaluacion()
    {
        // Primero evaluamos y almacenamos
        dbManager.EvaluarPosicion(this.gameObject);

        // Luego desactivamos el panel de "¿Estás listo?"
        panelListo.SetActive(false);

        // Ahora nos movemos a la Actividad4 (no PantallaDatos)
        SceneManager.LoadScene("Instruccion1");
    }

    // Método que se llama si el niño presiona "No"
    public void Reintentar()
    {
        panelListo.SetActive(false);
    }
}
