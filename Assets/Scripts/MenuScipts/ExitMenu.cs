using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public void OnClick_Exit() {
        MenuManager.OpenMenu(Menu.MAIN_MENU,gameObject);
    }
}
