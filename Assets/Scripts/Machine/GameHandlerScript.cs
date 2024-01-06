using UnityEngine;

public class GameHandlerScript : MonoBehaviour
{
    [SerializeField] private GameObject phase1;
    [SerializeField] private GameObject phase2;
    [SerializeField] private GameObject phase3;
    
    void Awake()
    {
        setPhaseActive();
    }

    private void setPhaseActive()
    {
        phase1.SetActive(false);
        phase2.SetActive(false);
        phase3.SetActive(false);

        int currentPhase = PlayerPrefs.GetInt("last_used_phase_number", 1);

        if(currentPhase == 1)
            phase1.SetActive(true);
        else if(currentPhase == 2)
            phase2.SetActive(true);
        else
            phase3.SetActive(true);
    }
}
