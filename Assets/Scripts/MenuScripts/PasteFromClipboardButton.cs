using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SubiNoOnibus.UI
{
    [RequireComponent(typeof(Button))]
    public class PasteFromClipboardButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private void Awake()
        {
#if UNITY_EDITOR
            UnityEngine.Assertions.Assert.IsNotNull(inputField, "Make sure to set the input field in " + gameObject.name);
#endif
#if UNITY_WEBGL
            gameObject.SetActive(false);
#else
            GetComponent<Button>().onClick.AddListener(OnClick_PasteFromClipboard);
#endif
        }

        private void OnClick_PasteFromClipboard()
        {
            inputField.SetTextWithoutNotify(GUIUtility.systemCopyBuffer);
            inputField.ForceLabelUpdate();
        }
    }
}