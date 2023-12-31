using TMPro;
using UnityEngine;

public class CreateInputUI : MonoBehaviour
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

    #region Create Input UI Buttons Util
    public void OnClick_SAVE() 
    {
        // get text components
        GameObject inputFileName_gameObject = transform.Find("Dynamic UI/File Name UI/Input Field/Text Area/Text").gameObject;
        inputFileName_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI inputFileName_TMP);
        GameObject inputFileContent_gameObject = transform.Find("Dynamic UI/File Content UI/Input Field/Text Area/Text").gameObject;
        inputFileContent_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI inputFileContent_TMP);
        
        // extract text
        string inputFileName = inputFileName_TMP.text;
        string inputFileContent = inputFileContent_TMP.text;
        
        // check if valid
        if(checkIfValid_fileName(inputFileName) && checkIfValid_fileContent(inputFileContent))
        {  
            SaveSystem.init();
            SaveSystem.saveFile("Inputs/" + inputFileName, inputFileContent);
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

    private bool checkIfValid_fileContent(string fileContent)
    {
        if(fileContent.Length <= 1)
        {
            // get components
            invalidFile_UI.transform.Find("Invalid File Text").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI title_TMP);
            invalidFile_UI.transform.Find("Invalid File Message").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI message_TMP);
            
            // set texts
            title_TMP.text = "Invalid File Name";
            message_TMP.text = "Cannot Save Empty File!";

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

    #region File Name or Content Invalid UI util
    
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
