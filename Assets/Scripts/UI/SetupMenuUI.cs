using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenuUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> phaseSprites;
    private MasterUIController masterUIController;
    private Image phaseImage;
    private TextMeshProUGUI phaseText;

    private const int NUMBER_OF_PHASES = 3;

    void Awake()
    {
        transform.parent.gameObject.TryGetComponent<MasterUIController>(out masterUIController);
        transform.Find("Dynamic UI/Select Phase UI/Phase Visual").TryGetComponent<Image>(out phaseImage);
        transform.Find("Dynamic UI/Select Phase UI/Switch Phase UI/Phase Text").TryGetComponent<TextMeshProUGUI>(out phaseText);
        
        Update_Phase_UI(PlayerPrefs.GetInt("last_used_phase_number", 1));
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
}
