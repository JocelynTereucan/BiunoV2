using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RememberCredentials : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Toggle rememberMeToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("RememberMe") && PlayerPrefs.GetInt("RememberMe") == 1)
        {
            emailInputField.text = PlayerPrefs.GetString("SavedEmail");
            passwordInputField.text = PlayerPrefs.GetString("SavedPassword");
            rememberMeToggle.isOn = true;
        }
    }

    public void SaveCredentials()
    {
        if (rememberMeToggle.isOn)
        {
            PlayerPrefs.SetString("SavedEmail", emailInputField.text);
            PlayerPrefs.SetString("SavedPassword", passwordInputField.text);
            PlayerPrefs.SetInt("RememberMe", 1);
        }
        else
        {
            PlayerPrefs.DeleteKey("SavedEmail");
            PlayerPrefs.DeleteKey("SavedPassword");
            PlayerPrefs.SetInt("RememberMe", 0);
        }
        PlayerPrefs.Save();
    }
}

