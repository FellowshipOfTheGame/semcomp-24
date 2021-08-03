using UnityEngine;
using UnityEngine.SceneManagement;
public static class MenuManager {
    public static bool isInitialised = false;
    private static GameObject mainMenu, settingsPanel, creditsMenu, storeMenu, rankingMenu;
    
    public static void Init() {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        panel = panel.transform.Find("PanelInit").gameObject;
        settingsPanel = panel.transform.Find("BottomMenu").transform.Find("SettingsPanel").gameObject;
        creditsMenu = panel.transform.Find("Credits").gameObject;
        mainMenu = panel.transform.Find("MainMenu").gameObject;
        storeMenu = panel.transform.Find("Store").gameObject;
        rankingMenu = panel.transform.Find("Ranking").gameObject;

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
            case Menu.STORE:
                storeMenu.SetActive(true);
                break;
            case Menu.RANKING:
                rankingMenu.SetActive(true);
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
