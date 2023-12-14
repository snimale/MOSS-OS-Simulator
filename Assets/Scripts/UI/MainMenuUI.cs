using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private MasterUIController masterUIController;

    void Awake()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);        
    }


    public void OnClick_QUIT() 
    {
        Application.Quit();    
    }

    public void OnClick_START() 
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    public void OnClick_SETTINGS() 
    {
        masterUIController.Switch_To_Settings_Mode();
    }
} 
