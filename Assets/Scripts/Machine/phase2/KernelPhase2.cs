using System.Text;
using UnityEngine;

public class KernelPhase2 : MonoBehaviour
{
    [SerializeField] private MemoryPhase2 memory;
    [SerializeField] private MemoryUIController2 memoryUIController2;
    [SerializeField] private CPUPhase2 cpu;
    [SerializeField] private InputFileObjectScript input;
    [SerializeField] private OutputFileObjectScript output;

    public void MOS()
    {
        int[] terminateCode = new int[2];
        terminateCode[0]=0; terminateCode[1]=0;

        if(cpu.get_SI() == 1 && cpu.get_TI() == 0)
            this.READ_ISR();
        else if(cpu.get_SI() == 2 && cpu.get_TI() == 0)
            this.WRITE_ISR();
        else if(cpu.get_SI() == 3 && cpu.get_TI() == 0)
            this.TERMINATE_ISR(terminateCode);
        else if(cpu.get_SI() == 1 && cpu.get_TI() == 2)
        {
            terminateCode[0] = 3;
            this.TERMINATE_ISR(terminateCode);
        } else if(cpu.get_SI() == 2 && cpu.get_TI() == 2)
        {
            this.WRITE_ISR();
            terminateCode[0] = 3;
            this.TERMINATE_ISR(terminateCode);
        }else if(cpu.get_SI() == 3 && cpu.get_TI() == 2)
            this.TERMINATE_ISR(terminateCode);
        else if(cpu.get_TI()==0 && cpu.get_PI()==1)
        {
            terminateCode[0] = 4;
            this.TERMINATE_ISR(terminateCode);
        } else if(cpu.get_TI()==0 && cpu.get_PI()==2)
        {
            terminateCode[0] = 5;
            this.TERMINATE_ISR(terminateCode);
        } else if(cpu.get_TI()==0 && cpu.get_PI()==3)
        {
            // check if valid page fault
            char[] ins = cpu.get_IR();
            if((ins[0] == 'G' && ins[1] == 'D') || (ins[0] == 'S' && ins[1] == 'R'))
            {
                // valid page fault
                int newPageBlockNumber = memory.getRandomUnallocatedBlock();
                int virtualAddress = (ins[2]-'0') * 10 + (ins[3]-'0');
                int pageTableAddress = (cpu.get_PTR()[0] * 10 + cpu.get_PTR()[1])*10;
                memory.set_byte(pageTableAddress + virtualAddress / 10, 2, (char)(newPageBlockNumber/10 + '0'));
                memory.set_byte(pageTableAddress + virtualAddress / 10, 3, (char)(newPageBlockNumber%10 + '0'));

                // decrement instruction counter
                char[] IC = cpu.get_IC();
                int IC_value = IC[0] * 10 + IC[1];
                IC_value -= 1;
                IC[0] = (char)(IC_value/10);
                IC[1] = (char)(IC_value%10);
                cpu.set_IC(IC);

                // its data page so add/update panel of block in Block Memory UI
                memoryUIController2.blockMemoryUITable_ADD_DATA(newPageBlockNumber);
            } else
            {
                // invalud page fault (PD, LR, CR)
                terminateCode[0] = 6;
                this.TERMINATE_ISR(terminateCode);    
            }
        } else if(cpu.get_TI()==2 && cpu.get_PI()==1)
        {
            terminateCode[0] = 3;
            terminateCode[1] = 4;
            this.TERMINATE_ISR(terminateCode);
        } else if(cpu.get_TI()==2 && cpu.get_PI()==2)
        {
            terminateCode[0] = 3;
            terminateCode[1] = 5;
            this.TERMINATE_ISR(terminateCode);
        } else
        {
            terminateCode[0] = 3;
            this.TERMINATE_ISR(terminateCode);
        }
        // unset interrupt flag after completion of ISR
        cpu.set_SI(0);
        cpu.set_PI(0);
        cpu.set_TI(0);
    }

    #region Interrupt Service Routines

    private void READ_ISR()
    {
        // get real address
        char[] IR = cpu.get_IR();
        int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
        int realAddress = cpu.addressMap(virtualAddress);
        realAddress = (realAddress / 10) * 10; // last digit 0

        string line = input.getNextInputLine();
        StringBuilder block = new StringBuilder();

        if(line[0]=='$' && line[0]=='E' && line[0]=='N' && line[0]=='D')
        {
            int[] terminateCode = new int[2];
            terminateCode[0]=1; terminateCode[1]=0;
            this.TERMINATE_ISR(terminateCode);
            return;
        }

        for(int i=0; i<10; i++)
        {
            for(int j=0; j<4; j++)
            {
                block.Append(memory.get_DEFAULT_MEMORY_CHARACTER());
            }
        }

        for(int i=0; i<line.Length; i++)
        {
            // Debug.Log((int)line[i]);
            if(line[i] == '\0' || line[i] == '\n' || line[i] == '\r' || i>=40) // byte limit of block = 40
                break;
            else
                block[i] = line[i];
        }
        // Debug.Log(block);
        memory.set_block(realAddress, block.ToString());
        // Debug.Log(memory.get_block(realAddress));
        cpu.set_SI(0);
    }

    private void WRITE_ISR()
    {
        // increment LLC & Terminate if over limit
        PCB pcb = cpu.get_PCB();
        pcb.set_LLC(pcb.get_LLC()+1);
        if(pcb.get_LLC() > pcb.get_TLL()) 
        {            
            int[] terminateCode = new int[2];
            terminateCode[0]=2; terminateCode[1]=0;
            this.TERMINATE_ISR(terminateCode);
            return;
        }

        // get real address
        char[] IR = cpu.get_IR();
        int virtualAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
        int realAddress = cpu.addressMap(virtualAddress);
        realAddress = (realAddress / 10) * 10; // last digit 0

        string block = memory.get_block(realAddress);
        StringBuilder line = new StringBuilder();

        for(int i=0; i<40; i++) // 40 = number of bytes in a block 
        {
            if(block[i] == memory.get_DEFAULT_MEMORY_CHARACTER() || block[i]=='\n')
                continue;
            else
                line.Append(block[i]);
        } line.Append("\n");

        output.appendTextInNewOutput(line.ToString());

        cpu.set_SI(0);
    }

    private void TERMINATE_ISR(int[] code)
    {
        // get terminate message string
        string starting = "R : "+ new string(cpu.get_R())
                                +", IR : "+ new string(cpu.get_IR())
                                +", IC : "+((char)(cpu.get_IC()[0]+'0')).ToString()+((char)(cpu.get_IC()[1]+'0')).ToString()
                                +", C : "+((char)(cpu.get_C()+'0')).ToString()
                                +", PTR : "+((char)(cpu.get_PTR()[0]+'0')).ToString()+((char)(cpu.get_PTR()[1]+'0')).ToString()+" ";
                                
        string error = "";
        for(int i=0; i<2; i++)
        {
            if(code[i] == 0 && i==0) error += "NO ERROR ";
            else if(code[i] == 1) error += "OUT OF DATA ";
            else if(code[i] == 2) error += "LINE LIMIT EXCEEDED ";
            else if(code[i] == 3) error += "TIME LIMIT EXCEEDED ";
            else if(code[i] == 4) error += "OPCODE ERROR ";
            else if(code[i] == 5) error += "OPERAND ERROR ";
            else if(code[i] == 6) error += "INVALID PAGE FAULT ";
        }
        string ending = "\n\n";
        string terminateSentence = starting + error + ending;
        
        output.appendTextInNewOutput(terminateSentence);
        if(code[0]==1)
        {
            // $END already read
            output.writeBackNewOutputFileContent();
        }


        // initialize cpu, memory again
        cpu.set_SI(0);
        cpu.initializeCPURegisters();
        memory.initializeMemory();
        memoryUIController2.initBlockMemoryUITable();


        // set is executing false
        cpu.set_isExecuting(false);
    }
    
    #endregion

}
