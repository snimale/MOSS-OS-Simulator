using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    private MasterUIController masterUIController;

    void Awake()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);        
    }

    public void OnClick_BACK() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }
}
