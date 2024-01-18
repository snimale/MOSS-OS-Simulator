using System.Text;
using UnityEngine;

public class MainPhase1 : MonoBehaviour
{
    #region  UI OBJECTS
    [SerializeField] private CPUUIController cpuUI;
    [SerializeField] private MemoryUIController memoryUI;
    [SerializeField] private KernelUIController kernelUI;
    #endregion
    [SerializeField] private InputFileObjectScript input;
    [SerializeField] private OutputFileObjectScript output;
    [SerializeField] private MemoryPhase1 memory;
    [SerializeField] private CPUPhase1 cpu;
    
    [SerializeField] private float loadTime; // time before the machin starts running
    private float initTime; // time when the machine was asked to start
    private int insBlockPTR; // stores the current instruction block number in program card
    private bool isProgramCard; // used to make sure, only program card is read into instruction blocks
    
    private void OnEnable() 
    {
        initTime = Time.time;
        insBlockPTR = 0;
        isProgramCard = false;
    }
    void Update()
    {
        if(Time.time - initTime < loadTime)
        {
            // wait for OnEnable of other classes to run
        }
        else if(!cpu.get_isExecuting()) 
        {
            string line = input.getNextInputLine();
            if(line[0] == '\0' || line[0] == '\n')
            {
               // end of phase 1 execution (end of file) or empty line

            } else if(line[0]=='$')
            {
                if(line[1]=='A' && line[2]=='M' && line[3]=='J')
                {
                    // initialize cpu, memory
                    cpu.set_SI(0);
                    cpu.initializeCPURegisters();
                    memory.initializeMemory();

                    // initialize new output text
                    output.initializeNewOutputFileContent();
                    cpu.set_isExecuting(false);
                    isProgramCard = true;

                    // initialize all UI
                    cpuUI.initializeCPUInfo();
                    memoryUI.updateContentTable();
                    memoryUI.updateBlockNumber();
                    kernelUI.initializeKernelInfo();
                }
                else if(line[1]=='E' && line[2]=='N' && line[3]=='D')
                {
                    insBlockPTR = 0;
                    output.writeBackNewOutputFileContent();
                }
                else if(line[1]=='D' && line[2]=='T' && line[3]=='A')
                {
                    isProgramCard = false;
                    cpu.set_isExecuting(true);
                }
            } else
            {
                if(isProgramCard)
                {
                    // make new block
                    StringBuilder insBlock = new StringBuilder();
                    for(int i=0; i<40; i++)
                        insBlock.Append(memory.get_DEFAULT_MEMORY_CHARACTER());
                    for(int i=0; i<40; i++)
                    {
                        if(line[i]=='\0')
                            break;
                        insBlock[i] = line[i];
                    }

                    memory.set_block(insBlockPTR, insBlock.ToString());
                    insBlockPTR += 10;
                } else {
                    // continue
                }
            }
        }
    }

    
}
