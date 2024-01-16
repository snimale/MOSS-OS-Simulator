using TMPro;
using UnityEngine;

public class KernelUIController2 : MonoBehaviour
{
    [SerializeField] private CPUPhase2 cpu;
    [SerializeField] private GameObject kernelUI;
    [SerializeField] private GameObject SIValueUI;
    [SerializeField] private GameObject TIValueUI;
    [SerializeField] private GameObject PIValueUI;
    
    void OnEnable()
    {
        initializeKernelInfo();
        kernelUI.SetActive(true);
    }

    private void initializeKernelInfo()
    {
        SIValueUI.GetComponent<TextMeshProUGUI>().text = "0";
        TIValueUI.GetComponent<TextMeshProUGUI>().text = "0";
        PIValueUI.GetComponent<TextMeshProUGUI>().text = "0";
    }

    public void updateKernelInfo()
    {
        SIValueUI.GetComponent<TextMeshProUGUI>().text = ((char)(cpu.get_SI()+'0')).ToString();
        TIValueUI.GetComponent<TextMeshProUGUI>().text = ((char)(cpu.get_TI()+'0')).ToString();
        PIValueUI.GetComponent<TextMeshProUGUI>().text = ((char)(cpu.get_PI()+'0')).ToString();
    }
}
