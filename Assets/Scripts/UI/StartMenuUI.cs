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
        masterUIController.Switch_To_SetupMenu_Mode();
    }

    public void OnClick_CREATE_INPUT() 
    {
        masterUIController.Switch_To_CreateInputMenu_Mode();
    }

    public void OnClick_CREATE_OUTPUT() 
    {
        masterUIController.Switch_To_CreateOutputMenu_Mode();
    }

    public void OnClick_DELETE_FILES() 
    {
        masterUIController.Switch_To_DeleteFilesMode();
    }

    public void OnClick_BACK() 
    {
        masterUIController.Switch_To_MainMenu_Mode();
    }
}
