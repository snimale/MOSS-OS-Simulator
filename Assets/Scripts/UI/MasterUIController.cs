using UnityEngine;

public class MasterUIController : MonoBehaviour
{
    private enum UIMode {MainMenu, Settings, StartMenu, SetupMenu};
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

        if(uiMode == UIMode.MainMenu)
            transform.Find("MainMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.Settings)
            transform.Find("Settings").gameObject.SetActive(true);
        else if(uiMode == UIMode.StartMenu)
            transform.Find("StartMenu").gameObject.SetActive(true);
        else if(uiMode == UIMode.SetupMenu)
            transform.Find("SetupMenu").gameObject.SetActive(true);
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
    #endregion

}
