using System.Text;
using UnityEngine;

public class KernelPhase1 : MonoBehaviour
{
    [SerializeField] private MemoryPhase1 memory;
    [SerializeField] private CPUPhase1 cpu;
    [SerializeField] private InputFileObjectScript input;
    [SerializeField] private OutputFileObjectScript output;

    public void MOS()
    {
        if(cpu.get_SI() == 1)
            this.READ_ISR();
        else if(cpu.get_SI() == 2)
            this.WRITE_ISR();
        else
            this.TERMINATE_ISR();
    }

    #region Interrupt Service Routines

    private void READ_ISR()
    {
        // get real address
        char[] IR = cpu.get_IR();
        int realAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
        realAddress = (realAddress / 10) * 10; // last digit 0

        string line = input.getNextInputLine();
        StringBuilder block = new StringBuilder();

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
        cpu.set_SI(0);
    }

    private void WRITE_ISR()
    {
        // get real address
        char[] IR = cpu.get_IR();
        int realAddress = (IR[2] - '0') * 10 + (IR[3] - '0');
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

    private void TERMINATE_ISR()
    {
        // add two newline to seperate two program cards output
        string terminateSentence = "\n\n";
        output.appendTextInNewOutput(terminateSentence);

        // initialize cpu, memory again
        cpu.set_SI(0);
        cpu.initializeCPURegisters();
        memory.initializeMemory();

        // set is executing false
        cpu.set_isExecuting(false);
    }
    
    #endregion

}
