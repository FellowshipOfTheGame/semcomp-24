using UnityEngine;
using UnityEngine.UI;

public class InputToggle : MonoBehaviour
{
    [SerializeField] private Toggle directionalToggle;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject leftDirectional;
    [SerializeField] private GameObject rightDirectional;

    private void Awake()
    {
        directionalToggle.onValueChanged.AddListener(Toogle);
        
        bool useDirectional = PlayerPrefs.GetInt("input", 0) == 1;
        directionalToggle.isOn = useDirectional;

        UpdateInputs(useDirectional);
    }

    public void Toogle(bool value)
    {
        PlayerPrefs.SetInt("input", value ? 1 : 0);
        UpdateInputs(value);
    }

    private void UpdateInputs(bool useDirectional)
    {
        if (useDirectional)
        {
            joystick.SetActive(false);
            leftDirectional.SetActive(true);
            rightDirectional.SetActive(true);
        }
        else
        {
            joystick.SetActive(true);
            leftDirectional.SetActive(false);
            rightDirectional.SetActive(false);
        }
    }
}
