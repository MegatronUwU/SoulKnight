using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
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
		toggleFixed.SetIsOnWithoutNotify(true);
        toggleDynamic.SetIsOnWithoutNotify(false);

		PlayerPrefs.SetString("JoystickType", "Fixed");
    }

    public void SetDynamicJoystick()
    {
        toggleFixed.SetIsOnWithoutNotify(false);
		toggleDynamic.SetIsOnWithoutNotify(true);

		PlayerPrefs.SetString("JoystickType", "Dynamic");
    }
}
