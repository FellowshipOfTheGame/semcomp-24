using UnityEngine;
using UnityEngine.SceneManagement;
public static class MenuManager 
{
    public static bool isInitialised = false;
    private static GameObject mainMenu, settingsPanel, creditsMenu, storeMenu, rankingMenu, loginMenu;
    
    public static void Init() 
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject panel = canvas.transform.Find("Panel").gameObject;
        panel = panel.transform.Find("PanelInit").gameObject;
        settingsPanel = panel.transform.Find("BottomMenu").transform.Find("SettingsPanel").gameObject;
        creditsMenu = panel.transform.Find("Credits").gameObject;
        mainMenu = panel.transform.Find("MainMenu").gameObject;
        storeMenu = panel.transform.Find("Store").gameObject;
        rankingMenu = panel.transform.Find("Ranking").gameObject;
        loginMenu = panel.transform.Find("LoginPanel").gameObject;

        isInitialised = true;
    }

    public static void OpenMenu(MenuEnum menu, GameObject callingMenu) 
    {

        if(!isInitialised)
            Init();

        switch(menu) 
        {
            case MenuEnum.MAIN_MENU:
                mainMenu.SetActive(true);
                break;
            case MenuEnum.CREDIT:
                creditsMenu.SetActive(true);
                break;
            case MenuEnum.STORE:
                storeMenu.SetActive(true);
                break;
            case MenuEnum.RANKING:
                rankingMenu.SetActive(true);
                break;
            case MenuEnum.LOGIN:
                loginMenu.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);
    }
   
    public static void PlayGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public static void SettingsPop() 
    {
        if(!isInitialised)
            Init();
        settingsPanel.GetComponent<Animator>().SetTrigger("Pop");
    }

    public static void QuitGame() 
    {
        Application.Quit();
    }
}
