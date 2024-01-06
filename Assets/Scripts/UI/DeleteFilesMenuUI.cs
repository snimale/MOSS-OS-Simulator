using UnityEngine;
using TMPro;

public class DeleteFilesMenuUI : MonoBehaviour
{
    private MasterUIController masterUIController;
    private GameObject deletionConfirmation_UI;
    private GameObject invalidFile_UI;
    private GameObject deletionSuccessful_UI;
    private GameObject deletionUnsuccessful_UI;

    public void OnEnable()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);     
        deletionConfirmation_UI = transform.Find("Deletion Confirmation UI").gameObject;   
        invalidFile_UI = transform.Find("Invalid File UI").gameObject;
        deletionSuccessful_UI = transform.Find("File Deleted Successfully UI").gameObject;
        deletionUnsuccessful_UI = transform.Find("File Deleted Unsuccessfully UI").gameObject;

        deletionConfirmation_UI.SetActive(false);
        invalidFile_UI.SetActive(false);
        deletionSuccessful_UI.SetActive(false);
        deletionUnsuccessful_UI.SetActive(false);
    }
    
    #region Delete Files Menu UI Buttons Util

    #region  File Type UI Button Util
    public void On_Click_CHANGE()
    {
        string previousFileType = PlayerPrefs.GetString("delete_file_type", "Input");
        string currentFileType = previousFileType == "Input" ? "Output" : "Input";
        PlayerPrefs.SetString("delete_file_type", currentFileType);
        UpdateFileTypeUI(currentFileType);
    }

    private void UpdateFileTypeUI(string fileType)
    {
        transform.Find("Dynamic UI/Choose File Type UI/File Type").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI TMP);
        TMP.text = fileType;
    }

    #endregion

    public void OnClick_DELETE()
    {
        if(checkIfValid_deleteFileName())
            deletionConfirmation_UI.SetActive(true);
    }

    public void OnClick_BACK()
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    private bool checkIfValid_deleteFileName()
    {
        // get text components
        GameObject deleteFileName_gameObject = transform.Find("Dynamic UI/File Name UI/Input Field/Text Area/Text").gameObject;
        deleteFileName_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI deleteFileName_TMP);
   
        // extract text
        string deleteFileName = deleteFileName_TMP.text;

        if(deleteFileName.Length <= 1 || deleteFileName.Contains("."))
        {
            // get components
            invalidFile_UI.transform.Find("Invalid File Text").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI title_TMP);
            invalidFile_UI.transform.Find("Invalid File Message").TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI message_TMP);
            
            // set texts
            title_TMP.text = "Invalid File Name";
            if(deleteFileName.Length <= 1)
                message_TMP.text = "File Name Cannot be Empty!";
            else
                message_TMP.text = "File Name Cannot Have \".\"!";

            // enable UI
            invalidFile_UI.SetActive(true);
            return false;
        }
        
        return true;
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

    #region Confirm Deletion UI

    public void OnClick_DeletionUI_CONTINUE() {
        // get text components
        GameObject deleteFileName_gameObject = transform.Find("Dynamic UI/File Name UI/Input Field/Text Area/Text").gameObject;
        deleteFileName_gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI deleteFileName_TMP);
   
        // extract text
        string deleteFileName = deleteFileName_TMP.text;

        // append file type
        deleteFileName = PlayerPrefs.GetString("delete_file_type", "Input") + "s/" + deleteFileName;

        // delete file
        bool deletionSuccessful = FileHandler.deleteFile(deleteFileName);

        if(deletionSuccessful)
            deletionSuccessful_UI.SetActive(true);
        else
            deletionUnsuccessful_UI.SetActive(true);

        deletionConfirmation_UI.SetActive(false);
    }

    public void OnClick_DeletionUI_BACK()
    {
        deletionConfirmation_UI.SetActive(false);
    }

    #endregion

    #region File Deleted Successfully UI

    public void OnClick_GO_TO_START_MENU()
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    public void OnClick_DELETE_NEW_FILE()
    {
        deletionSuccessful_UI.SetActive(false);
    }

    #endregion

    #region File Deleted Unsuccessfully UI

    public void OnClick_BACK_TO_START_MENU()
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    public void OnClick_TRY_AGAIN()
    {
        deletionUnsuccessful_UI.SetActive(false);
    }

    #endregion

}
