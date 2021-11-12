using SubiNoOnibus.UI;
using UnityEngine.Networking;

namespace SubiNoOnibus.Backend
{
    public static class DefaultErrorHandling
    {
        public static void OnGameScene(UnityWebRequest request)
        {
            if (request.responseCode >= 500)
            {
                MainMenu.ExitGame();
            }
            else if (request.responseCode >= 400)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("StartupMenu");
            }
        }

        public static void OnMenuScene(UnityWebRequest request, IMenu actualMenu, MainMenu mainMenu)
        {
            if (request.responseCode >= 500)
            {
                MainMenu.ExitGame();
            }
            else if (request.responseCode >= 400)
            {
                actualMenu.Close();
                mainMenu.OpenLoginMenu();
            }
        }
    }
}
