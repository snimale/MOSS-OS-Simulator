using TMPro;
using UnityEngine;

public class CPUUIController : MonoBehaviour
{
    [SerializeField] private CPUPhase1 cpuPhase1;
    [SerializeField] private GameObject cpuUI;
    [SerializeField] private GameObject RValueUI;
    [SerializeField] private GameObject IRValueUI;
    [SerializeField] private GameObject ICValueUI;
    [SerializeField] private GameObject CValueUI;
    void OnEnable()
    {
        initializeCPUInfo();
        cpuUI.SetActive(true);
    }

    private void initializeCPUInfo()
    {
        RValueUI.GetComponent<TextMeshProUGUI>().text = "0000";
        IRValueUI.GetComponent<TextMeshProUGUI>().text = "0000";
        ICValueUI.GetComponent<TextMeshProUGUI>().text = "00";
        CValueUI.GetComponent<TextMeshProUGUI>().text = "0";
    }

    public void updateCPUInfo()
    {
        RValueUI.GetComponent<TextMeshProUGUI>().text = new string (cpuPhase1.get_R());
        IRValueUI.GetComponent<TextMeshProUGUI>().text = new string (cpuPhase1.get_IR());
        ICValueUI.GetComponent<TextMeshProUGUI>().text = new string (cpuPhase1.get_IC());
        CValueUI.GetComponent<TextMeshProUGUI>().text = cpuPhase1.get_C().ToString();
    }
}
