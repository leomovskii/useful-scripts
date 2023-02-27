using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 Joystick for your needs. Create object in ierarchy:
 1. Transparent image with this script
 2. Child image 'background'
 3. Child child image 'handle'
 4. Configure what you need
 5. Use
*/

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	enum Direction {
		Both, Horizontal, Vertical
	}

    private RectTransform _BaseRect;
    private Camera _Camera;
    private Canvas _Canvas;
    private Vector2 _Input;

	private Vector2 _InitialPosition;

    [SerializeField] private RectTransform m_Background;
    [SerializeField] private RectTransform m_Handle;
    [SerializeField] private float m_HandleRange = 0.65f;
    [SerializeField] private bool m_HideIdleJoystick;
    [SerializeField] private Direction m_Direction;
	[Space(10)]
    [SerializeField] private Vector2 m_PressedAnchor = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 m_ReleasedAnchor = new Vector2(0.5f, 0.5f);

	public Vector2 input => _Input;

	private void Start() {
		_BaseRect = GetComponent<RectTransform>();
		_Canvas = GetComponentInParent<Canvas>();
		_Camera = _Canvas.worldCamera;
		_InitialPosition = m_Background.anchoredPosition;
		if (m_HideIdleJoystick)
			m_Background.gameObject.SetActive(false);
	}

	public void OnPointerDown(PointerEventData eventData) {
		SetupBackground(m_PressedAnchor);

		if (m_HideIdleJoystick)
			m_Background.gameObject.SetActive(true);

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_BaseRect, eventData.position, _Camera, out Vector2 point))
			m_Background.anchoredPosition = point - (m_Background.anchorMax * _BaseRect.sizeDelta) + _BaseRect.pivot * _BaseRect.sizeDelta;
		else
			m_Background.anchoredPosition = Vector2.zero;

		OnDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData) {
		Vector2 position = RectTransformUtility.WorldToScreenPoint(_Camera, m_Background.position);
		Vector2 radius = m_Background.sizeDelta / 2;
		_Input = (eventData.position - position) / radius;
		
		if (m_Direction == Direction.Horizontal)
			_Input.y = 0;
		else if (m_Direction == Direction.Vertical)
			_Input.x = 0;
		
		m_Handle.anchoredPosition = (_Input.magnitude > 1 ? _Input.normalized : _Input) * radius * m_HandleRange;
	}

	public void OnPointerUp(PointerEventData eventData) {
		_Input = Vector2.zero;
		SetupBackground(m_ReleasedAnchor);
		m_Handle.anchoredPosition = Vector2.zero;
		m_Background.anchoredPosition = _InitialPosition;
		if (m_HideIdleJoystick)
			m_Background.gameObject.SetActive(false);
	}

	private void SetupBackground(Vector2 vector) {
        m_Background.anchorMin = vector;
        m_Background.anchorMax = vector;
        m_Background.pivot = vector;
	}
}
