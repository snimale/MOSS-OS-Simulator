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
    
    private void OnEnable()
    {
        initializeCPUInfo();
        cpuUI.SetActive(true);
    }
    public void initializeCPUInfo()
    {
        RValueUI.GetComponent<TextMeshProUGUI>().text = "0000";
        IRValueUI.GetComponent<TextMeshProUGUI>().text = "0000";
        ICValueUI.GetComponent<TextMeshProUGUI>().text = "00";
        CValueUI.GetComponent<TextMeshProUGUI>().text = "0";
    }
    public void updateCPUInfo()
    {
        // Before update, int_IC changed to char_IC
        char[] char_IC = cpuPhase1.get_IC(); // gets integer values
        for(int i=0; i<char_IC.Length; i++)
            char_IC[i] += '0';
        
        // set new text in UI
        RValueUI.GetComponent<TextMeshProUGUI>().text = new string(cpuPhase1.get_R());
        IRValueUI.GetComponent<TextMeshProUGUI>().text = new string(cpuPhase1.get_IR());
        ICValueUI.GetComponent<TextMeshProUGUI>().text = new string(char_IC); 
        CValueUI.GetComponent<TextMeshProUGUI>().text = ((char)(cpuPhase1.get_C()+'0')).ToString();
    }
}
