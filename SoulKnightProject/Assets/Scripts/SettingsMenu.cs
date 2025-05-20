using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Toggle _toggleFixed;
    [SerializeField] private Toggle _toggleDynamic;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Slider _volumeSlider;


    private void Start()
    {
		_settingsPanel.SetActive(false);

		string savedType = PlayerPrefs.GetString("JoystickType", "Fixed");
        if (savedType == "Dynamic")
        {
            _toggleDynamic.isOn = true;
            _toggleFixed.isOn = false;
            SetDynamicJoystick();
        }
        else
        {
            _toggleFixed.isOn = true;
            _toggleDynamic.isOn = false;
            SetFixedJoystick();
        }

        if (_volumeSlider != null) { 

            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            _volumeSlider.value = savedVolume;
            SoundManager.Instance.SetVolume(savedVolume);
        
            _volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            Debug.LogWarning("Volume est null");
        }


	}

	public void ToggleSettingsPanel()
    {
        _settingsPanel.SetActive(!_settingsPanel.activeSelf);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
    }

    public void SetFixedJoystick()
    {
		_toggleFixed.SetIsOnWithoutNotify(true);
        _toggleDynamic.SetIsOnWithoutNotify(false);

		PlayerPrefs.SetString("JoystickType", "Fixed");
    }

    public void SetDynamicJoystick()
    {
        _toggleFixed.SetIsOnWithoutNotify(false);
		_toggleDynamic.SetIsOnWithoutNotify(true);

		PlayerPrefs.SetString("JoystickType", "Dynamic");
    }

    private void SetVolume(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }
}
