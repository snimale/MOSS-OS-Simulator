using System.Text;
using UnityEngine;

public class CPUPhase1 : MonoBehaviour
{
    [SerializeField] private MemoryPhase1 memoryPhase1;
    [SerializeField] private KernelPhase1 kernelPhase1;
    [SerializeField] private float executionLatency;
    private char[] R;
    private char[] IR; // will contain current instruction (character values only)
    private char[] IC; // will point to next instruction (integer value)
    private char C; // integer value 0/1
    
    private int SI;
    private bool isExecuting; // is on when $DTA occurs
    private float lastExecutionTime;

    private void OnEnable() 
    {
        initializeCPURegisters();
        SI = 0;
        lastExecutionTime = Time.time;
        isExecuting = false;
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
        
        for(int i=0; i<4; i++)
        {
            R[i] = '0';
            IR[i] = '0';
        }

        for(int i=0; i<2; i++)
        {
            IC[i] = (char) 0;
        }

        C = (char) 0;
    }

    #region GET FUNCTION
    
    #region Registers

    public char[] get_R()
    {
        return R;
    } 

    public char[] get_IR()
    {
        return IR;
    }

    public char[] get_IC()
    {
        return IC;
    }

    public char get_C()
    {
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

    #endregion
    public void execute()
    {
        
        // fetch
        int currAddress = IC[0] * 10 + IC[1];
        int nextAddress = currAddress+1;
        IC[0] = (char) (nextAddress / 10);
        IC[1] = (char) (nextAddress % 10);

        string currInstruction = memoryPhase1.get_word(currAddress);
        for(int i=0; i<4; i++)
            IR[i] = currInstruction[i];

        // Debug.Log((char)(IC[0]+'0')+""+(char)(IC[1]+'0')+" : "+IR[0]+IR[1]+IR[2]+IR[3]);

        // decode & execute
        if(IR[0]=='G' && IR[1]=='D')
        {
            this.set_SI(1);
        } else if(IR[0]=='P' && IR[1]=='D')
        {
            this.set_SI(2);
        } else if(IR[0]=='L' && IR[1]=='R')
        {
            // get real address
            int realAddress = (IR[2] - '0') * 10 + (IR[3] - '0');

            // fetch word
            string word = memoryPhase1.get_word(realAddress);

            // set register
            for(int i=0; i<4; i++)
            {
                R[i] = word[i];
            }

        } else if(IR[0]=='S' && IR[1]=='R')
        {
            // get real address
            int realAddress = (IR[2] - '0') * 10 + (IR[3] - '0');

            // get word
            StringBuilder word = new StringBuilder();
            for(int i=0; i<4; i++)
            {
                word.Append(R[i]);
            }

            // set in memory
            memoryPhase1.set_word(realAddress, word.ToString());

        } else if(IR[0]=='C' && IR[1]=='R')
        {
            // get real address
            int realAddress = (IR[2] - '0') * 10 + (IR[3] - '0');

            // get word
            string word = memoryPhase1.get_word(realAddress);

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
            //Debug.Log("Terminate");
            this.set_SI(3);
        }

        if(this.get_SI() != 0)
        {
            kernelPhase1.MOS();
        }
    }
}
