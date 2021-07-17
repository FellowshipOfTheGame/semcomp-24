using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Credits() {
        MenuManager.OpenMenu(Menu.CREDIT,gameObject);
    }
    public void OnClick_Store() {
        MenuManager.OpenMenu(Menu.STORE,gameObject);
    }
    public void OnClick_Ranking() {
        MenuManager.OpenMenu(Menu.RANKING,gameObject);
    }
}
