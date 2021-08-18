using SubiNoOnibus.Networking.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public partial class InsertKeyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LoginMenu loginMenu;
    [SerializeField] private PopupMessageWindow logInterface;

    [Header("UI Fields")]
    [SerializeField] private TMPro.TMP_InputField keyInputField;

    [Header("Error Messages")]
    [SerializeField] private string invalidFieldMsg;
    [SerializeField] private string expiredCodeMsg;
    [SerializeField] private string serverErrorMsg;

    private void OnClick_SwitchToLogin()
    {

    }
    private void SwitchToMainMenu()
    {
    }

    

    
}
