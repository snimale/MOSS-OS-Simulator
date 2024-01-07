using TMPro;
using UnityEngine;

public class KernelUIController : MonoBehaviour
{
    [SerializeField] private CPUPhase1 cpu;
    [SerializeField] private GameObject kernelUI;
    [SerializeField] private GameObject SIValueUI;
    void OnEnable()
    {
        initializeKernelInfo();
        kernelUI.SetActive(true);
    }

    private void initializeKernelInfo()
    {
        SIValueUI.GetComponent<TextMeshProUGUI>().text = "0";
    }

    public void updateKernelInfo()
    {
        SIValueUI.GetComponent<TextMeshProUGUI>().text = ((char)(cpu.get_SI()+'0')).ToString();
    }
}
