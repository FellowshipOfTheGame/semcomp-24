using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Credits() 
    {
        MenuManager.OpenMenu(MenuEnum.CREDIT,gameObject);
    }
    public void OnClick_Store() 
    {
        MenuManager.OpenMenu(MenuEnum.STORE,gameObject);
    }
    public void OnClick_Ranking() 
    {
        MenuManager.OpenMenu(MenuEnum.RANKING,gameObject);
    }

    public void Logout()
    {
        MenuManager.OpenMenu(MenuEnum.LOGIN, gameObject);
    }
}
