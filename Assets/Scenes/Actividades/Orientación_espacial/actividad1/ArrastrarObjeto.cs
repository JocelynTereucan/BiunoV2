using UnityEngine;
using UnityEngine.EventSystems;

public class ArrastrarUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;
    private bool arrastrando = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );
        offset = rectTransform.anchoredPosition - localPoint;
        arrastrando = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!arrastrando || canvas == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        rectTransform.anchoredPosition = localPoint + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        arrastrando = false;
    }
}
