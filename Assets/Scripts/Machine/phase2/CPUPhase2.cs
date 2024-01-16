using System.Text;
using UnityEngine;

public class CPUPhase2 : MonoBehaviour
{
    [SerializeField] private MemoryPhase2 memory;
    [SerializeField] private KernelPhase2 kernel;
    [SerializeField] private CPUUIController2 cpuUIController;
    [SerializeField] private KernelUIController2 kernelUIController;
    private char[] R;
    private char[] IR; // will contain current instruction (character values only)
    private char[] IC; // will point to next instruction (integer value)
    private char[] PTR; // will point to 
    private char C; // integer value 0/1
    
    private int SI;
    private int PI;
    private int TI;
    private bool isExecuting; // is on when $DTA occurs
    private float lastExecutionTime;
    private float executionLatency; // time between two successive execution
    private PCB currentPCB;

    private void OnEnable() 
    {
        initializeCPURegisters();
        SI = 0; PI = 0; TI = 0;
        lastExecutionTime = Time.time;
        isExecuting = false;
        executionLatency = PlayerPrefs.GetFloat("machine_execution_latency", 0.1f);
    }

    private void Update()
    {
        if(isExecuting && Time.time - lastExecutionTime >= executionLatency)
        {
            execute();
            lastExecutionTime = Time.time;
        }
    }

    public void initializeCPURegisters()
    {
        R = new char[4];
        IR = new char[4];
        IC = new char[2];
        PTR = new char[2];
        
        for(int i=0; i<4; i++)
        {
            R[i] = '0';
            IR[i] = '0';
        }

        for(int i=0; i<2; i++)
        {
            IC[i] = (char) 0;
            PTR[i] = (char) 0;
        }

        C = (char) 0;
    }

    public int addressMap(int currVirtualAddress)
    {
        int pageTableBlockNumber = (PTR[0] * 10) + PTR[1];
        string pageTableEntry = memory.get_word(pageTableBlockNumber * 10 + currVirtualAddress / 10);

        // add real block address and the word offset
        int currRealAddress = ((pageTableEntry[2] - '0') * 10 + (pageTableEntry[3] - '0')) * 10 + currVirtualAddress % 10;
        return currRealAddress;
    }

    private bool checkPageFault(int virtualAddress)
    {
        int pageTableBlockNumber = (PTR[0] * 10) + PTR[1];
        string pageTableEntry = memory.get_word(pageTableBlockNumber * 10 + virtualAddress / 10);
        // Debug.Log(pageTableEntry);

        if(pageTableEntry[2] == memory.get_DEFAULT_PAGE_TABLE_CHARACTER() && pageTableEntry[2] == memory.get_DEFAULT_PAGE_TABLE_CHARACTER())
        {
            // page table not contain entry for this block -> page fault
            return true;
        } else
        {
            // valid page entry exists
            return false;
        }
    }

    #region GET FUNCTION
    
    #region Registers

    public char[] get_R()
    {
        // we return copy, to avoid pass by reference
        char[] RCopy = new char[4];
        R.CopyTo(RCopy, 0);
        return RCopy;
    } 

    public char[] get_IR()
    {
        // we return copy, to avoid pass by reference
        char[] IRCopy = new char[4];
        IR.CopyTo(IRCopy, 0);
        return IRCopy;
    }

    public char[] get_IC()
    {
        // we return copy, to avoid pass by reference
        char[] ICCopy = new char[2];
        IC.CopyTo(ICCopy, 0);
        return ICCopy;
    }

    public char[] get_PTR()
    {
        // we return copy, to avoid pass by reference
        char[] PTRcopy = new char[2];
        PTR.CopyTo(PTRcopy, 0);
        return PTRcopy;
    }

    public char get_C()
    {
        // no need to copy and return, as char is not pass by reference
        return C;
    }

    #endregion

    #region Execution Information
    
    public bool get_isExecuting()
    {
        return isExecuting;
    }

    public int get_SI()
    {
        return SI;
    }

    public int get_TI()
    {
        return TI;
    }

    public int get_PI()
    {
        return PI;
    }

    public PCB get_PCB()
    {
        return currentPCB;
    }

    #endregion
    
    #endregion

    #region SET FUNCTIONS

    public void set_isExecuting(bool value)
    {
        isExecuting = value;   
    }

    public void set_SI(int value)
    {
        SI = value;   
    }

    public void set_PI(int value)
    {   
        PI = value;
    }

    public void set_TI(int value)
    {
        TI = value;
    }

    public void set_PTR(int blockNumber)
    {
        PTR[0] = (char) (blockNumber / 10);
        PTR[1] = (char) (blockNumber % 10);
    }

    public void setCurrentPCB(PCB pcb)
    {
        currentPCB = pcb;
    }

    public void set_IC(char[] newIC)
    {
        IC[0] = newIC[0];
        IC[1] = newIC[1];
    }

    #endregion
    public void execute()
    {
        
    // fetch
        // get virtual address & increment IC
        int currVirtualAddress = IC[0] * 10 + IC[1];
        int nextVirtualAddress = currVirtualAddress+1;
        IC[0] = (char) (nextVirtualAddress / 10);
        IC[1] = (char) (nextVirtualAddress % 10);

        // get real address from virtual address
        int currRealAddress = addressMap(currVirtualAddress);
        string currInstruction = memory.get_word(currRealAddress);
        for(int i=0; i<4; i++)
            IR[i] = currInstruction[i];

        // Debug.Log((char)(IC[0]+'0')+""+(char)(IC[1]+'0')+" : "+IR[0]+IR[1]+IR[2]+IR[3]);

        // decode & execute
        if(!(IR[2] >= '0' && IR[2] <= '9') || !(IR[3] >= '0' && IR[3] <= '9'))
        {
            // invalid operand
            PI = 2;
        } else if(IR[0]=='G' && IR[1]=='D')
        {
            int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
            if(checkPageFault(virtualAddress))
                PI = 3;
            else
                this.set_SI(1);
        } else if(IR[0]=='P' && IR[1]=='D')
        {
            int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
            if(checkPageFault(virtualAddress))
                PI = 3;
            else
                this.set_SI(2);

        } else if(IR[0]=='L' && IR[1]=='R')
        {
            int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
            if(checkPageFault(virtualAddress))
                PI = 3; // page fault
            else
            {
                // get real address
                int realAddress = addressMap(virtualAddress);

                // fetch word
                string word = memory.get_word(realAddress);

                // set register
                for(int i=0; i<4; i++)
                {
                    R[i] = word[i];
                }
            }
        } else if(IR[0]=='S' && IR[1]=='R')
        {
            int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
            if(checkPageFault(virtualAddress))
                PI = 3; // page fault
            else
            {
                // get real address
                int realAddress = addressMap(virtualAddress);

                // get word
                StringBuilder word = new StringBuilder();
                for(int i=0; i<4; i++)
                {
                    word.Append(R[i]);
                }

                // set in memory
                memory.set_word(realAddress, word.ToString());
            }
        } else if(IR[0]=='C' && IR[1]=='R')
        {
            int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
            if(checkPageFault(virtualAddress))
                PI = 3; // page fault
            else{

                // get word
                int realAddress = addressMap(virtualAddress);
                string word = memory.get_word(realAddress);

                // compare word
                bool isEqual = true;
                for(int i=0; i<4; i++)
                {
                    if(R[i] != word[i])
                        isEqual = false;
                }

                if(isEqual)
                    C = (char) 1;
                else 
                    C = (char) 0;

            }
        } else if(IR[0]=='B' && IR[1]=='T')
        {
            if(C == (char) 1)
            {
                // branch
                IC[0] = (char) (IR[2]-'0');
                IC[1] = (char) (IR[3]-'0');
            } 
            C = (char) 0; // reset C after consuming it
        
        } else if(IR[0]=='H')
        {
            this.set_SI(3);
        } else
        {
            // invalid opcode
            PI = 1;
        }

        // add one run time to PCB & check for end of time limit for this process
        currentPCB.set_TTC(currentPCB.get_TTC() + 1);
        if(currentPCB.get_TTC() > currentPCB.get_TTL())
            TI = 2;

        // write back
        cpuUIController.updateCPUInfo();
        kernelUIController.updateKernelInfo();
    
        // check for interrupts
        if(SI != 0 || TI != 0 || PI != 0)
        {
            // we update UI first, then check for interrupt
            // this is done as MOS will set SI back to 0, before even update SI value in UI.
            kernel.MOS();
        }

    }
}
