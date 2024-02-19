using TMPro;
using UnityEngine;

public class EditFilesUI : MonoBehaviour
{
    private MasterUIController masterUIController;
    private GameObject fileNotSaved_UI;
    private GameObject fileSaved_UI;
    private GameObject invalidFile_UI;
    private GameObject beforeSelectionUI;
    private GameObject afterSelectionUI;

    private string filename;
    private string editFileType;

    public void OnEnable()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);     
        fileNotSaved_UI = transform.Find("File Will Not Be Saved UI").gameObject;   
        fileSaved_UI = transform.Find("File Saved Successfully UI").gameObject;
        invalidFile_UI = transform.Find("Invalid File UI").gameObject;
        beforeSelectionUI = transform.Find("BeforeSelection").gameObject;
        afterSelectionUI = transform.Find("AfterSelection").gameObject;

        filename = "default";
        editFileType = "Input";

        fileNotSaved_UI.SetActive(false);
        fileSaved_UI.SetActive(false);
        invalidFile_UI.SetActive(false);
        afterSelectionUI.SetActive(false);

        beforeSelectionUI.SetActive(true);
    }

    #region switch before-after selection mode
    
    public void switchToAfterSelection() {
        // get text components
        GameObject inputFileName_gameObject = transform.Find("BeforeSelection/File Name UI/Input Field/Text Area/Text").gameObject;
        inputFileName_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI inputFileName_TMP);
        GameObject inputFileType_gameObject = transform.Find("BeforeSelection/File Type UI/Label").gameObject;
        inputFileType_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI inputFileType_TMP);

        // extract text
        string inputFileName = inputFileName_TMP.text;
        string inputFileType = inputFileType_TMP.text;
        
        // remove end char from raw text
        inputFileName = inputFileName.Remove(inputFileName.Length-1, 1);

        // check if valid
        if(checkIfValid_fileName(inputFileName))
        { 
            // get file details
            filename = inputFileName;
            editFileType = inputFileType;

            // get text components
            GameObject inputFileContent_gameObject1 = transform.Find("AfterSelection/Dynamic UI/File Content UI/Input Field/Text Area/Placeholder").gameObject;
            inputFileContent_gameObject1.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI inputFileContent_TMP1);
            GameObject inputFileContent_gameObject2 = transform.Find("AfterSelection/Dynamic UI/File Content UI/Input Field").gameObject;
            inputFileContent_gameObject2.TryGetComponent<TMP_InputField>(out TMP_InputField inputFileContent_TMP2);
            
            // get content
            string text = FileHandler.loadFile(getFilePath());

            // set content
            inputFileContent_TMP1.text = text; 
            inputFileContent_TMP2.text = text;

            // switch UI
            beforeSelectionUI.SetActive(false);
            afterSelectionUI.SetActive(true);
        }
    }
    public void switchtoBeforeSelection() {
        afterSelectionUI.SetActive(false);
        beforeSelectionUI.SetActive(true);
    }
    
    #endregion 

    #region Edit Files UI Buttons Util
    public void OnClick_SAVE() 
    {
        // get text components
        GameObject inputFileContent_gameObject = transform.Find("AfterSelection/Dynamic UI/File Content UI/Input Field/Text Area/Text").gameObject;
        inputFileContent_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI inputFileContent_TMP);
        
        // extract text
        string inputFileContent = inputFileContent_TMP.text;
        
        // remove end char from raw text
        inputFileContent = inputFileContent.Remove(inputFileContent.Length-1, 1);

        // check if valid
        if(checkIfValid_fileContent(inputFileContent))
        {
            
            
            FileHandler.init();
            FileHandler.saveFile(getFilePath(), inputFileContent);
            
            // enable UI
            fileSaved_UI.SetActive(true);
        }
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
    public void OnClick_BACK_BEFORE_EDIT()
    {
        OnClick_GO_TO_START_MENU();
    }
    private string getFilePath()
    {
        // get path to save to
        string path;
        if(editFileType == "Input")
            path = "Inputs/" + filename;
        else
            path = "Outputs/" + filename;
        return path;
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

    public void OnClick_EDIT_A_NEW_FILE()
    {
        fileSaved_UI.SetActive(false);
        switchtoBeforeSelection();
    }
    #endregion

}
