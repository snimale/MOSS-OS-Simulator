using System.Text;
using UnityEngine;

public class MemoryPhase1 : MonoBehaviour
{
    [SerializeField] private MemoryUIController memoryUIController;
    private char[, ] memory;
    private const char DEFAULT_MEMORY_CHARACTER = '`'; 
    private const int MEMORY_BLOCK_COUNT = 10;
    private const int MEMORY_WORD_PER_BLOCK = 10;
    private const int MEMORY_WORD_COUNT = MEMORY_BLOCK_COUNT * MEMORY_WORD_PER_BLOCK;
    private const int MEMORY_BYTE_PER_WORD = 4;

    private void OnEnable()
    {
        // define and initialize memory with DEFAULT_MEMORY_CHARACTER
        memory = new char[MEMORY_WORD_COUNT, MEMORY_BYTE_PER_WORD];
        initializeMemory();
    }

    public void initializeMemory()
    {
        for(int i=0; i<MEMORY_WORD_COUNT; i++)
        {
            for(int j=0; j<MEMORY_BYTE_PER_WORD; j++)
                memory[i, j] = DEFAULT_MEMORY_CHARACTER;
        }
    }
    public void printMemory()
    {
        for(int i=0; i<10; i++)
        {
            Debug.Log(memory[i,0].ToString());
        }
    }

    #region get functions
    
    public string get_word(int realAddress)
    {
        StringBuilder word = new StringBuilder();
        for(int i=0; i<MEMORY_BYTE_PER_WORD; i++)
            word.Append(memory[realAddress, i]);
        
        return word.ToString();
    }

    public string get_block(int realAddress)
    {
        // assumes realAddress is multiple of 10
        StringBuilder block = new StringBuilder();
        for(int i=0; i<MEMORY_WORD_PER_BLOCK; i++)
        {
            for(int j=0; j<MEMORY_BYTE_PER_WORD; j++)
                block.Append(memory[realAddress + i, j]);
        } 
        
        return block.ToString();
    }

    public char get_DEFAULT_MEMORY_CHARACTER()
    {
        return DEFAULT_MEMORY_CHARACTER;
    }

    public int get_MEMORY_WORD_PER_BLOCK()
    {
        return MEMORY_WORD_PER_BLOCK;
    }

    public int get_MEMORY_BYTE_PER_WORD()
    {
        return MEMORY_BYTE_PER_WORD;
    }

    public int get_MEMORY_BLOCK_COUNT()
    {
        return MEMORY_BLOCK_COUNT;
    }

    #endregion

    #region set functions

    public void set_word(int realAddress, string text)
    {
        for(int i=0; i<MEMORY_BYTE_PER_WORD; i++)
            memory[realAddress, i] = text[i];

        memoryUIController.updateContentTable();
    }

    public void set_block(int realAddress, string block)
    {
        for(int i=0; i<MEMORY_WORD_PER_BLOCK; i++)
        {
            for(int j=0; j<MEMORY_BYTE_PER_WORD; j++)
                memory[realAddress + i, j] = block[i * 4 + j];
        }

        memoryUIController.updateContentTable();
    }

    public void set_byte(int realAddress, int byteOffset, char value)
    {
        memory[realAddress, byteOffset] = value;
    
        memoryUIController.updateContentTable();
    }

    #endregion

}
