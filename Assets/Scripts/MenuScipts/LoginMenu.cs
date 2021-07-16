using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    private GameObject panelInit;
    GameObject button;

    private void Start() {
        panelInit = GameObject.Find("Canvas");
        panelInit = panelInit.transform.Find("Panel").gameObject;
        panelInit = panelInit.transform.Find("PanelInit").gameObject;
        button = gameObject.transform.GetChild(1).gameObject;
    }

    public void OnClick_Login() {
        panelInit.SetActive(true);
        button.SetActive(false);
    }
}
