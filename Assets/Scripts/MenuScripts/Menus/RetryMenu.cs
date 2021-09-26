using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SubiNoOnibus.UI
{
    public class RetryMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private RaceManager raceManager;

        [SerializeField] private TMPro.TextMeshProUGUI titleMesh;
        [SerializeField] private TMPro.TextMeshProUGUI textMesh;

        [SerializeField] private Button retryBtn;
        [SerializeField] private Button cancelBtn;
        
        private int _numTries = 0;

        private void Awake()
        {
            retryBtn.onClick.AddListener(Retry);           
            cancelBtn.onClick.AddListener(Cancel);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            cancelBtn.interactable = true;
            retryBtn.interactable = true;
        }

        public void SessionExpired()
        {
            _numTries++;
            titleMesh.SetText($"Sessão expirada!");
            textMesh.SetText($"Volte ao menu e faça login.");
            retryBtn.interactable = false;
        }

        public void InternetConnectionLost()
        {
            _numTries++;
            titleMesh.SetText($"Erro de Conexão! ({_numTries}x)");
            textMesh.SetText($"Não foi possível conectar com nossa API.");
        }

        public void Retry()
        {
            cancelBtn.interactable = false;
            retryBtn.interactable = false;
            raceManager.EndRace();
        }

        public void Cancel()
        {
            SceneManager.LoadScene("StartupMenu");
        }
    }
}
