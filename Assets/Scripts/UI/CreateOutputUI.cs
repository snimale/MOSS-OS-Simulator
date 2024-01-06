using TMPro;
using UnityEngine;

public class CreateOutputUI : MonoBehaviour
{
    private MasterUIController masterUIController;
    private GameObject fileNotSaved_UI;
    private GameObject fileSaved_UI;
    private GameObject invalidFile_UI;

    public void OnEnable()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);     
        fileNotSaved_UI = transform.Find("File Will Not Be Saved UI").gameObject;   
        fileSaved_UI = transform.Find("File Saved Successfully UI").gameObject;
        invalidFile_UI = transform.Find("Invalid File UI").gameObject;

        fileNotSaved_UI.SetActive(false);
        fileSaved_UI.SetActive(false);
        invalidFile_UI.SetActive(false);
    }

    #region Create Output UI Buttons Util

    public void OnClick_SAVE() 
    {
        // get text components
        GameObject outputFileName_gameObject = transform.Find("Dynamic UI/File Name UI/Input Field/Text Area/Text").gameObject;
        outputFileName_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI outputFileName_TMP);
   
        // extract text
        string outputFileName = outputFileName_TMP.text;
        
        // check if valid & save
        if(checkIfValid_fileName(outputFileName))
        {  
            FileHandler.init();
            FileHandler.saveFile("Outputs/" + outputFileName, "");
        }

        // enable UI
        fileSaved_UI.SetActive(true);
    }

    private bool checkIfValid_fileName(string fileName)
    {

        if(fileName.Length <= 1 || fileName.Contains("."))
        {
            // get components
            invalidFile_UI.transform.Find("Invalid File Text").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI title_TMP);
            invalidFile_UI.transform.Find("Invalid File Message").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI message_TMP);
            
            // set texts
            title_TMP.text = "Invalid File Name";
            if(fileName.Length <= 1)
                message_TMP.text = "File Name Cannot be Empty!";
            else
                message_TMP.text = "File Name Cannot Have \".\"!";

            // enable UI
            invalidFile_UI.SetActive(true);
            return false;
        }
        
        return true;
    }

    public void OnClick_BACK() 
    {
        // this button will only work if file has not been saved
        fileNotSaved_UI.SetActive(true); 
    }

    #region File Name Invalid UI util
    
    public void OnClick_GO_BACK_AND_CHANGE()
    {
        // get components
        invalidFile_UI.transform.Find("Invalid File Text").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI title_TMP);
        invalidFile_UI.transform.Find("Invalid File Message").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI message_TMP);
    
        // reset texts
        title_TMP.text = "";
        message_TMP.text = "";

        // disable UI
        invalidFile_UI.SetActive(false);
    }

    #endregion

    #endregion

    #region File Will Not Be Saved UI util
    // This Region Functions Will Only Run When fileNotSaved_UI is set active.
    public void OnClick_CONTINUE_WITHOUT_SAVING()
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    public void OnClick_GO_BACK_AND_SAVE()
    {
        fileNotSaved_UI.SetActive(false);
    }

    #endregion

    #region File Saved Successfully UI util
    // This Region Functions Will Only Run When fileSaved_UI is set active.
    public void OnClick_GO_TO_START_MENU()
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    public void OnClick_CREATE_A_NEW_FILE()
    {
        fileSaved_UI.SetActive(false);
    }
    #endregion

}
