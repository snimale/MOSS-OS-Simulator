using UnityEngine;

public class MasterUIController : MonoBehaviour
{
    private enum UIMode {MainMenu, Settings, StartMenu, SetupMenu, CreateInputMenu, CreateOutputMenu, EditFilesMenu, DeleteFilesMenu};
    private UIMode uiMode;

    void Awake()
    {
        uiMode = UIMode.MainMenu;
        Update_UI();
    }

    private void Update_UI() 
    {
        transform.Find("MainMenu").gameObject.SetActive(false);
        transform.Find("Settings").gameObject.SetActive(false);
        transform.Find("StartMenu").gameObject.SetActive(false);
        transform.Find("SetupMenu").gameObject.SetActive(false);
        transform.Find("CreateOutputMenu").gameObject.SetActive(false);
        transform.Find("CreateInputMenu").gameObject.SetActive(false);
        transform.Find("EditFilesMenu").gameObject.SetActive(false);
        transform.Find("DeleteFilesMenu").gameObject.SetActive(false);


        if(uiMode == UIMode.MainMenu)
            transform.Find("MainMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.Settings)
            transform.Find("Settings").gameObject.SetActive(true);
        else if(uiMode == UIMode.StartMenu)
            transform.Find("StartMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.SetupMenu)
            transform.Find("SetupMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.CreateInputMenu)
            transform.Find("CreateInputMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.CreateOutputMenu)
            transform.Find("CreateOutputMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.EditFilesMenu)
            transform.Find("EditFilesMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.DeleteFilesMenu)
            transform.Find("DeleteFilesMenu").gameObject.SetActive(true);
    }
    
    #region UI Mode Switch Functions
    public void Switch_To_MainMenu_Mode() 
    {
        uiMode = UIMode.MainMenu;
        Update_UI();
    }
    
    public void Switch_To_Settings_Mode()
    {
        uiMode = UIMode.Settings;
        Update_UI();
    }

    public void Switch_To_StartMenu_Mode()
    {
        uiMode = UIMode.StartMenu;
        Update_UI();
    }

    public void Switch_To_SetupMenu_Mode()
    {
        uiMode = UIMode.SetupMenu;
        Update_UI();
    }

    public void Switch_To_CreateInputMenu_Mode()
    {
        uiMode = UIMode.CreateInputMenu;
        Update_UI();
    }

    public void Switch_To_CreateOutputMenu_Mode()
    {
        uiMode = UIMode.CreateOutputMenu;
        Update_UI();
    }

    public void Switch_To_EditFilesMenu_Mode()
    {
        uiMode = UIMode.EditFilesMenu;
        Update_UI();
    }

    public void Switch_To_DeleteFilesMode()
    {
        uiMode = UIMode.DeleteFilesMenu;
        Update_UI();
    }

    #endregion

}
