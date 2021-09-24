using UnityEngine;
using UnityEngine.UI;

public class InputToggle : MonoBehaviour
{
    [SerializeField] private Toggle joystickToggle;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject leftDirectional;
    [SerializeField] private GameObject rightDirectional;

    private void Awake()
    {
        joystickToggle.onValueChanged.AddListener(Toogle);
        
        bool useJoystick = PlayerPrefs.GetInt("input", 0) == 1;
        joystickToggle.isOn = useJoystick;

        UpdateInputs(useJoystick);
    }

    public void Toogle(bool value)
    {
        PlayerPrefs.SetInt("input", value ? 1 : 0);
        UpdateInputs(value);
    }

    private void UpdateInputs(bool useJoystick)
    {
        if (useJoystick)
        {
            joystick.SetActive(true);
            leftDirectional.SetActive(false);
            rightDirectional.SetActive(false);
        }
        else
        {
            joystick.SetActive(false);
            leftDirectional.SetActive(true);
            rightDirectional.SetActive(true);
        }
    }
}
