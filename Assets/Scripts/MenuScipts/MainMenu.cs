using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SubiNoOnibus.Networking.Requests;

namespace SubiNoOnibus.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LoginMenu loginMenu;

        [Header("Buttons")]
        [SerializeField] private Button playBtn;
        [SerializeField] private Button logoutBtn;
        [SerializeField] private Button exitBtn;

        private void Awake()
        {
            SetupButtonsCallback();
        }

        private void SetupButtonsCallback()
        {
            playBtn.onClick.AddListener(PlayGame);
            logoutBtn.onClick.AddListener(Logout);
            exitBtn.onClick.AddListener(ExitGame);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Logout()
        {
            StartCoroutine(UserAuthRequestHandler.Logout(OpenLoginMenu, (req) => OpenLoginMenu()));
        }

        private void OpenLoginMenu()
        {
            loginMenu.Open();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}