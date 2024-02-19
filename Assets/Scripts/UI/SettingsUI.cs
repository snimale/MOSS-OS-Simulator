using TMPro;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    private MasterUIController masterUIController;

    void Awake()
    {        
        init();
    }


    private void init() {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);
        updateOperatingSpeedPreferenceUI();
    }

    private void updateOperatingSpeedPreferenceUI() {
        // get text mesh pro text component
        TextMeshProUGUI operatingSpeedText = transform.Find("Dynamic UI/Scroll/Scroll Area/Preferences Layout Group/Operating Speed/text").GetComponent<TextMeshProUGUI>();
        
        // update text with current value of operating speed
        operatingSpeedText.text = "Operating Speed : " + PlayerPrefs.GetFloat("machine_execution_latency", 0.1f).ToString("F1");
    }

    public void OnClick_BACK() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }

    public void OnClick_OPERATING_SPEED()
    {
        // get current operating speed
        float currentOperatingSpeed = PlayerPrefs.GetFloat("machine_execution_latency", 0.1f);

        // find new operating speed
        float newOperatingSpeed = currentOperatingSpeed + 0.1f;
        if(newOperatingSpeed>5.0f)
            newOperatingSpeed = 0.1f; // reset if > 1.0f
        
        // set current operating speed as new operating speed
        PlayerPrefs.SetFloat("machine_execution_latency", newOperatingSpeed);

        // update ui
        updateOperatingSpeedPreferenceUI();
    }
}
