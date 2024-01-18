using System.Text;
using UnityEngine;

public class MainPhase2 : MonoBehaviour
{
    [SerializeField] private InputFileObjectScript input;
    [SerializeField] private OutputFileObjectScript output;
    [SerializeField] private MemoryPhase2 memory;
    [SerializeField] private MemoryUIController2 memoryUIController2;
    [SerializeField] private CPUPhase2 cpu;
    
    [SerializeField] private float loadTime;
    private float initTime;
    private int insBlockPTR; // in phase 2 it will point to virtual memory
    private bool isProgramCard;
    
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
            //Debug.Log(line);
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
                    
                    // make pcb
                    PCB pcb = makePCB(line);
                    cpu.setCurrentPCB(pcb);

                    // make page table
                    int pageTableBlockNumber = memory.getRandomUnallocatedBlock();
                    memory.initializePageTable(pageTableBlockNumber);
                    cpu.set_PTR(pageTableBlockNumber);

                    // update Block Type in Block Memory UI
                    memoryUIController2.initBlockMemoryUITable();
                    memoryUIController2.blockMemoryUITable_ADD_PAGE_TABLE(pageTableBlockNumber);


                    // initialize new output text
                    output.initializeNewOutputFileContent();
                    cpu.set_isExecuting(false);
                    isProgramCard = true;
                    insBlockPTR = 0;
                }
                else if(line[1]=='E' && line[2]=='N' && line[3]=='D')
                {
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
                    
                    // get program card into block
                    for(int i=0; i<40; i++)
                    {
                        if(line[i]=='\0')
                            break;
                        insBlock[i] = line[i];
                    }

                    // get new page & put ins block there
                    int blockNumber = memory.getRandomUnallocatedBlock();
                    memory.set_block(blockNumber * 10, insBlock.ToString());
                    
                    // update page table and virtual memory pointer    
                    char[] PTR = cpu.get_PTR();
                    int pageTableBlockNumber = PTR[0] * 10 + PTR[1];
                    memory.set_byte(pageTableBlockNumber * 10 + insBlockPTR / 10, 2, (char) (blockNumber / 10 + '0'));
                    memory.set_byte(pageTableBlockNumber * 10 + insBlockPTR / 10, 3, (char) (blockNumber % 10 + '0'));
                    insBlockPTR += 10;

                    // update the Block type for Block Memory UI
                    memoryUIController2.blockMemoryUITable_ADD_INSTRUCTION(blockNumber);
                } else {
                    // continue
                    //Debug.Log(".");
                }
            }
        }
    }

    private PCB makePCB(string line)
    {
        int PID = (line[4]-'0')*1000+(line[5]-'0')*100+(line[6]-'0')*10+(line[7]-'0');
        int TTL = (line[8]-'0')*1000+(line[9]-'0')*100+(line[10]-'0')*10+(line[11]-'0');
        int TLL = (line[12]-'0')*1000+(line[13]-'0')*100+(line[14]-'0')*10+(line[15]-'0');
        int TTC = 0, LLC = 0;
        PCB pcb = new PCB(PID, TTL, TLL, TTC, LLC);
        return pcb;
    }

    
}
