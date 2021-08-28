using UnityEngine;
using UnityEngine.SceneManagement;
public static class MenuManager {
    public static bool isInitialised = false;
    private static GameObject mainMenu, settingsPanel, creditsMenu;
    
    public static void Init() {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        settingsPanel = panel.transform.Find("BottomMenu").transform.Find("SettingsPanel").gameObject;
        creditsMenu = panel.transform.Find("Credits").gameObject;
        mainMenu = panel.transform.Find("MainMenu").gameObject;

        isInitialised = true;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu) {

        if(!isInitialised)
            Init();

        switch(menu) {
            case Menu.MAIN_MENU:
                mainMenu.SetActive(true);
                break;
            case Menu.CREDIT:
                creditsMenu.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);
    }
   
    public static void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public static void SettingsPop() {
        if(!isInitialised)
            Init();
        settingsPanel.GetComponent<Animator>().SetTrigger("Pop");
    }

    public static void QuitGame() {
        Application.Quit();
    }
}
