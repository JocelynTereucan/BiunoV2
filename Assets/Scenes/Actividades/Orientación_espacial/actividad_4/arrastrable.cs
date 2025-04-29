using UnityEngine;
using UnityEngine.EventSystems;

public class Arrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool arrastrando = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        arrastrando = true;
        canvasGroup.blocksRaycasts = false;
        Actividad4Manager.Instance.EmpezarArrastre();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        arrastrando = false;
        canvasGroup.blocksRaycasts = true;

        Actividad4Manager.Instance.ObjetoMovido(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si se hace clic, reproducir el audio
        Actividad4Manager.Instance.ObjetoTocado(gameObject);
    }
}
