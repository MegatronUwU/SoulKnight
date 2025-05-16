using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Joystick fixedJoystick;
    [SerializeField] private Joystick dynamicJoystick;

    [SerializeField] private Toggle toggleFixed;
    [SerializeField] private Toggle toggleDynamic;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        settingsPanel.SetActive(false);

        string savedType = PlayerPrefs.GetString("JoystickType", "Fixed");
        if (savedType == "Dynamic")
        {
            toggleDynamic.isOn = true;
            toggleFixed.isOn = false;
            SetDynamicJoystick();
        }
        else
        {
            toggleFixed.isOn = true;
            toggleDynamic.isOn = false;
            SetFixedJoystick();
        }
    }

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void SetFixedJoystick()
    {
        if (!toggleFixed.isOn) return;

        fixedJoystick.gameObject.SetActive(true);
        dynamicJoystick.gameObject.SetActive(false);
        toggleDynamic.isOn = false;

        PlayerPrefs.SetString("JoystickType", "Fixed");
    }

    public void SetDynamicJoystick()
    {
        if (!toggleDynamic.isOn) return;

        fixedJoystick.gameObject.SetActive(false);
        dynamicJoystick.gameObject.SetActive(true);
        toggleFixed.isOn = false;

        PlayerPrefs.SetString("JoystickType", "Dynamic");
    }
}
