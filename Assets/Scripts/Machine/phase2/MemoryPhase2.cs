using System.Text;
using TMPro;
using UnityEngine;

public class MemoryPhase2 : MonoBehaviour
{
    [SerializeField] private MemoryUIController2 memoryUIController;
    private char[, ] memory;
    private int[] allocated; // used to track the allocated and unallocated blocks of memory
    private const char DEFAULT_MEMORY_CHARACTER = '`'; 
    private const char DEFAULT_PAGE_TABLE_CHARACTER = '$';
    private const int MEMORY_BLOCK_COUNT = 30;
    private const int MEMORY_WORD_PER_BLOCK = 10;
    private const int MEMORY_WORD_COUNT = MEMORY_BLOCK_COUNT * MEMORY_WORD_PER_BLOCK;
    private const int MEMORY_BYTE_PER_WORD = 4;

    private void OnEnable()
    {
        // define and initialize memory with DEFAULT_MEMORY_CHARACTER
        memory = new char[MEMORY_WORD_COUNT, MEMORY_BYTE_PER_WORD];
        allocated = new int[MEMORY_BLOCK_COUNT];
        initializeMemory();
    }

    public int getRandomUnallocatedBlock()
    {
        // returns a random block from memory which is unallocated
        
        System.Random r = new System.Random();
        int randomBlockNumber = -1;
        bool done = false;

        while(!done)
        {
            randomBlockNumber = r.Next(0, MEMORY_BLOCK_COUNT);
            
            // if block is unallocated, then allocate it and return
            if(allocated[randomBlockNumber] == 0)
            {
                allocated[randomBlockNumber] = 1;
                done = true;
            }
        }

        return randomBlockNumber;
    }

    public void initializeMemory()
    {
        // init by default character
        for(int i=0; i<MEMORY_WORD_COUNT; i++)
        {
            for(int j=0; j<MEMORY_BYTE_PER_WORD; j++)
                memory[i, j] = DEFAULT_MEMORY_CHARACTER;
        }

        // set blocks allocated -> unallocated
        for(int i=0; i<MEMORY_BLOCK_COUNT; i++)
        {
            allocated[i] = 0;
        }
    }

    public void initializePageTable(int blockNumber)
    {
        for(int i=0; i<MEMORY_WORD_PER_BLOCK; i++)
        {
            for(int j=0; j<MEMORY_BYTE_PER_WORD; j++)
            {
                memory[blockNumber * MEMORY_WORD_PER_BLOCK + i, j] = DEFAULT_PAGE_TABLE_CHARACTER;
            }
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

    public char get_DEFAULT_PAGE_TABLE_CHARACTER()
    {
        return DEFAULT_PAGE_TABLE_CHARACTER;
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
        //Debug.Log(realAddress);
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
