using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if SUBI_NO_ONIBUS_ONLINE
using Backend = SubiNoOnibus.Backend.Online.Requests;
#else
using Backend = SubiNoOnibus.Backend.Offline.Requests;
#endif


namespace SubiNoOnibus.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LoginMenu loginMenu;

        [Header("Buttons")]
        [SerializeField] private Button playBtn;
        [SerializeField] private Button logoutBtn;
        [SerializeField] private Button semcompBtn;

        private void Awake()
        {
            SetupButtonsCallback();
            StartCoroutine(loginMenu.ValidateCookie());
        }

        private void SetupButtonsCallback()
        {
            playBtn.onClick.AddListener(PlayGame);
            logoutBtn.onClick.AddListener(Logout);
            semcompBtn.onClick.AddListener(OpenSemcompSite);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Logout()
        {
            StartCoroutine(Backend::UserAuthRequestHandler.Logout(OpenLoginMenu, (req) => OpenLoginMenu()));
        }

        public void OpenLoginMenu()
        {
            loginMenu.Open();
        }

        public void OpenSemcompSite()
        {
            WebLink.OpenLinkJSPlugin("https://semcomp.icmc.usp.br");
        }

        public static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}