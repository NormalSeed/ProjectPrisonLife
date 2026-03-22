using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;
    [SerializeField] private float _maxRadius = 100f;

    public Vector2 InputDirection { get; private set; }

    private int _touchId = -1;

    private void Awake()
    {
        _background.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_touchId != -1) return;

        _touchId = eventData.pointerId;
        _background.position = eventData.position;
        _background.gameObject.SetActive(true);
        _handle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != _touchId) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, _maxRadius);
        _handle.anchoredPosition = clamped;
        InputDirection = clamped / _maxRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != _touchId) return;

        _touchId = -1;
        _background.gameObject.SetActive(false);
        _handle.anchoredPosition = Vector2.zero;
        InputDirection = Vector2.zero;
    }
}
