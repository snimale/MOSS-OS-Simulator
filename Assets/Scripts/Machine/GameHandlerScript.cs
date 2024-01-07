using UnityEngine;

public class GameHandlerScript : MonoBehaviour
{
    [SerializeField] private GameObject phase1;
    [SerializeField] private GameObject phase1UI;
    [SerializeField] private GameObject phase2;
    [SerializeField] private GameObject phase2UI;
    [SerializeField] private GameObject phase3;
    [SerializeField] private GameObject phase3UI;
    
    void Awake()
    {
        setPhaseActive();
    }

    private void setPhaseActive()
    {
        phase1.SetActive(false);
        phase2.SetActive(false);
        phase3.SetActive(false);

        phase1UI.SetActive(false);
        phase2UI.SetActive(false);
        phase3UI.SetActive(false);

        int currentPhase = PlayerPrefs.GetInt("last_used_phase_number", 1);

        if(currentPhase == 1)
        {
            phase1.SetActive(true);
            phase1UI.SetActive(true);
        }
        else if(currentPhase == 2) 
        {
            phase2.SetActive(true);
            phase2UI.SetActive(true);
        }
        else
        {
            phase3.SetActive(true);
            phase3UI.SetActive(true);
        }
    }
}
