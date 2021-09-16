using UnityEngine;
using UnityEngine.EventSystems;

namespace SubiNoOnibus.UI
{
	public class PressHandler : MonoBehaviour, IPointerDownHandler
	{
		public UnityEngine.Events.UnityEvent OnPress;

        public void OnPointerDown(PointerEventData eventData)
		{
			OnPress?.Invoke();
		}
	}
}