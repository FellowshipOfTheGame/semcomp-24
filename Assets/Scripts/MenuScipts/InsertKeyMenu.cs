using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InsertKeyMenu : MonoBehaviour
{
    [SerializeField] private GameObject loginMenuPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("UI Fields")]
    [SerializeField] private TMPro.TMP_InputField keyInputField;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnClick_InsertKey);
    }
    public void OnClick_BackToLogin()
    {
        loginMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClick_InsertKey()
    {
        Debug.Log(keyInputField.text);
        StartCoroutine(GetSession(keyInputField.text));   
    }

    public IEnumerator GetSession(string key)
    {
        GetSessionData data = new GetSessionData { code = key };
        using UnityWebRequest request = WebRequestFactory.PostJson(Endpoints.get_session_url, JsonUtility.ToJson(data));
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);   
        }
        else
        {
            string cookie = request.GetResponseHeader("Set-Cookie");
            Debug.Log(cookie);
            PlayerPrefs.SetString("Auth", cookie);

            OpenMainMenuPanel();
        }
        Debug.Log(request.downloadHandler.text);
    }

    private void OpenMainMenuPanel()
    {
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    [System.Serializable]
    private struct GetSessionData
    {
        public string code;
    }
}
