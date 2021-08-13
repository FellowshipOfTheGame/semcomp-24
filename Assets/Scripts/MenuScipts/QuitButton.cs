using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnClick_Quit() 
    {
        Debug.Log("Quit");
        MenuManager.QuitGame();
    }
}
