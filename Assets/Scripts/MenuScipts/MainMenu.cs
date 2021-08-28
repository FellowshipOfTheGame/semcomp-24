using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Credits() {
        MenuManager.OpenMenu(Menu.CREDIT,gameObject);
    }
}
