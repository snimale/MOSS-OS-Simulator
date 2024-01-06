using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetupMenuUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> phaseSprites;
    private MasterUIController masterUIController;
    private Image phaseImage;
    private TextMeshProUGUI phaseText;

    private GameObject errorUI;
    private GameObject inputUI;
    private GameObject outputUI;

    private string inputFileName;
    private string outputFileName;
    private const int NUMBER_OF_PHASES = 3;

    private void Awake()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);
        transform.Find("Dynamic UI/Select Phase UI/Phase Visual").TryGetComponent<Image>(out phaseImage);
        transform.Find("Dynamic UI/Select Phase UI/Switch Phase UI/Phase Text").TryGetComponent<TextMeshProUGUI>(out phaseText);
        
        Update_Phase_UI(PlayerPrefs.GetInt("last_used_phase_number", 1));

        errorUI = transform.Find("Error UI").gameObject;
        inputUI = transform.Find("Select Input UI").gameObject;
        outputUI = transform.Find("Select Output UI").gameObject;
    }

    private void OnEnable()
    {
        errorUI.SetActive(false);
        inputUI.SetActive(false);
        outputUI.SetActive(false);

        inputFileName = "";
        outputFileName = "";
    }

    #region Phase Switch UI
    public void OnClick_NEXT_PHASE() 
    {
        // find next phase
        int last_used_phase_number = PlayerPrefs.GetInt("last_used_phase_number", 1);
        int next_phase_number = ((last_used_phase_number-1)+1)%NUMBER_OF_PHASES + 1;
        PlayerPrefs.SetInt("last_used_phase_number", next_phase_number);

        // update phase information in UI
        Update_Phase_UI(next_phase_number);
    }

    public void OnClick_PREVIOUS_PHASE() 
    {
        // find prev phase
        int last_used_phase_number = PlayerPrefs.GetInt("last_used_phase_number", 1);
        int prev_phase_number = ((last_used_phase_number-1)-1+NUMBER_OF_PHASES)%NUMBER_OF_PHASES + 1;
        PlayerPrefs.SetInt("last_used_phase_number", prev_phase_number);
        
        // update phase information in UI
        Update_Phase_UI(prev_phase_number);
    }

    private void Update_Phase_UI(int phase) 
    {
        phaseImage.overrideSprite = phaseSprites[phase - 1];
        phaseText.text = "Phase: " + phase.ToString();
    }
    #endregion

    #region Buttons UI
    public void OnClick_BACK() 
    {
        masterUIController.Switch_To_StartMenu_Mode();
    }

    public void OnClick_SELECT_INPUT() 
    {
        inputUI.SetActive(true);
    }

    public void OnClick_SETECT_OUTPUT() 
    {
        outputUI.SetActive(true);
    }

    public void OnClick_START()
    {
        if(inputFileName == "")
        {
            changeErrorMessage("No Input File Found!");
            errorUI.SetActive(true);
        } 
        else if(outputFileName == "")
        {
            changeErrorMessage("No Output File Found!");
            errorUI.SetActive(true);
        } else
        {
            PlayerPrefs.SetString("input_file_name", inputFileName);
            PlayerPrefs.SetString("output_file_name", outputFileName);
            Debug.Log(inputFileName);
            SceneManager.LoadScene("Machine");
        }
    }
    #endregion

    #region Error Message UI

    public void OnClick_ErrorMessage_BACK()
    {
        errorUI.SetActive(false);
    }

    private void changeErrorMessage(string newText)
    {
        GameObject errorMessageObject = errorUI.transform.Find("Error Message Text").gameObject;
        errorMessageObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI TMP);
        if(TMP!=null)
            TMP.text = newText;
    }

    #endregion

    #region  Select Input UI
    
    public void OnClick_selectInputUI_SAVE()
    {
        // get component
        TextMeshProUGUI TMP = transform.Find("Select Input UI/Dynamic UI/InputField/Text Area/Text").gameObject.GetComponent<TextMeshProUGUI>();
        inputFileName = TMP.text;
        if(TMP.text.Length <= 1)
            inputFileName = "";
        else
            inputFileName = "Inputs/" + inputFileName;
    }

    public void OnClick_selectInputUI_BACK()
    {
        inputUI.SetActive(false);
    }

    #endregion

    #region Select Output UI

    public void OnClick_selectOutputUI_SAVE()
    {
        // get component
        TextMeshProUGUI TMP = transform.Find("Select Output UI/Dynamic UI/InputField/Text Area/Text").gameObject.GetComponent<TextMeshProUGUI>();
        outputFileName = TMP.text;
        if(TMP.text.Length <= 1)
            outputFileName = "";
        else
            outputFileName = "Outputs/" + outputFileName;
    }

    public void OnClick_selectOutputUI_BACK()
    {
        outputUI.SetActive(false);
    }

    #endregion
}
