using System;
using TMPro;
using UnityEngine;

public class MemoryUIController : MonoBehaviour
{
    [SerializeField] private MemoryPhase1 memory;
    [SerializeField] private GameObject memoryUIBlockValue;
    [SerializeField] private GameObject fullMemoryUI;
    [SerializeField] private GameObject miniMemoryUI;
    private int blockNumber;

    private void OnEnable()
    {
        setUI_miniMemory();
        blockNumber = 0; // zero indexed
    }

    public void setUI_fullMemory()
    {
        fullMemoryUI.SetActive(true);
        miniMemoryUI.SetActive(false);
    }
    
    public void setUI_miniMemory()
    {
        miniMemoryUI.SetActive(true);
        fullMemoryUI.SetActive(false);
    }

    #region FULL UI FUNCTIONS 
    public void OnClick_NEXT()
    {
        blockNumber = (blockNumber + 1) % memory.get_MEMORY_BLOCK_COUNT();
        updateBlockNumber();
        updateContentTable();
    }

    public void OnClick_PREV()
    {
        blockNumber = (blockNumber - 1 + memory.get_MEMORY_BLOCK_COUNT()) % memory.get_MEMORY_BLOCK_COUNT();
        updateBlockNumber();
        updateContentTable();
    }

    public void updateBlockNumber()
    {
        TextMeshProUGUI blockNumberText = memoryUIBlockValue.GetComponent<TextMeshProUGUI>();
        blockNumberText.text = blockNumber.ToString();
    }

    public void updateContentTable()
    {
        string block = memory.get_block(blockNumber * 10);

        for(int i=0; i<memory.get_MEMORY_WORD_PER_BLOCK(); i++)
        {
            string wordObjectName = "Word (" + (i+1).ToString() + ")";
            string indexObjectName = "Index (" + (i+1).ToString() + ")";
            GameObject wordObject = fullMemoryUI.transform.Find("Content Table/"+wordObjectName).gameObject;
            GameObject indexObject = fullMemoryUI.transform.Find("Content Table Index/"+indexObjectName).gameObject;

            indexObject.GetComponent<TextMeshProUGUI>().text = String.Format("{0:000}", blockNumber * 10 + i);

            for(int j=0; j<memory.get_MEMORY_BYTE_PER_WORD(); j++)
            {
                string byteObjectName = "Byte (" + (j+1).ToString() + ")";
                GameObject ByteObject = wordObject.transform.Find(byteObjectName).gameObject;

                ByteObject.GetComponent<TextMeshProUGUI>().text = block[i * 4 + j].ToString();
            } 
        }
    }
    #endregion
}
