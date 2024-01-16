using TMPro;
using UnityEngine;

public class CPUUIController2 : MonoBehaviour
{
    [SerializeField] private CPUPhase2 cpu;
    [SerializeField] private GameObject cpuUI;
    [SerializeField] private GameObject RValueUI;
    [SerializeField] private GameObject IRValueUI;
    [SerializeField] private GameObject ICValueUI;
    [SerializeField] private GameObject CValueUI;
    [SerializeField] private GameObject PTRValueUI;
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
        PTRValueUI.GetComponent<TextMeshProUGUI>().text = "0000";
    }

    public void updateCPUInfo()
    {
        // Before update, int_IC changed to char_IC
        char[] char_IC = cpu.get_IC(); // gets integer values
        for(int i=0; i<char_IC.Length; i++)
            char_IC[i] += '0';
        
        // Before update, int_IC changed to char_IC
        char[] char_PTR = cpu.get_PTR(); // gets integer values
        for(int i=0; i<char_PTR.Length; i++)
            char_PTR[i] += '0';

        // set new text in UI
        RValueUI.GetComponent<TextMeshProUGUI>().text = new string(cpu.get_R());
        IRValueUI.GetComponent<TextMeshProUGUI>().text = new string(cpu.get_IR());
        ICValueUI.GetComponent<TextMeshProUGUI>().text = new string(char_IC); 
        CValueUI.GetComponent<TextMeshProUGUI>().text = ((char)(cpu.get_C()+'0')).ToString();
        PTRValueUI.GetComponent<TextMeshProUGUI>().text = new string(char_PTR);
    }
}
