using UnityEngine;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown tmpDropdown;

    void Start()
    {
        tmpDropdown.onValueChanged.AddListener(delegate {
            TMPDropdownValueChanged(tmpDropdown);
        });
    }

    private void SetResolution(int width, int height, bool fullScreen)
    {
        Screen.SetResolution(width, height, fullScreen);
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    void TMPDropdownValueChanged(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                SetResolution(1280, 720, Screen.fullScreen);

                break;
            case 1:
                SetResolution(1600, 900, Screen.fullScreen);

                break;
            case 2:
                SetResolution(1920, 1080, Screen.fullScreen);

                break;
        }
    }
}
