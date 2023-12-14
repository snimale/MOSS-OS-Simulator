using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    private MasterUIController masterUIController;

    void Awake()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);        
    }

    public void OnClick_RUN_MACHINE() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }

    public void OnClick_CREATE_INPUT() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }

    public void OnClick_CREATE_OUTPUT() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }

    public void OnClick_BACK() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }
}
