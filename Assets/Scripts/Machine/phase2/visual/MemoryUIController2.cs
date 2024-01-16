using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryUIController2 : MonoBehaviour
{
    [SerializeField] private MemoryPhase2 memory;
    [SerializeField] private GameObject memoryUIBlockValue;
    [SerializeField] private GameObject fullMemoryUI;
    [SerializeField] private GameObject miniMemoryUI;
    [SerializeField] private GameObject blockMemoryUI;
    [SerializeField] private GameObject switchUITypePage;
    private int blockNumber;

    private void OnEnable()
    {
        setUI_miniMemory();
        blockNumber = 0; // zero indexed
        initBlockMemoryUITable();
    }

    #region UI TYPE SWITCH FUNCTIONS

    public void setUI_fullMemory()
    {
        fullMemoryUI.SetActive(true);
        miniMemoryUI.SetActive(false);
        blockMemoryUI.SetActive(false);
        switchUITypePage.SetActive(false);
    }
    
    public void setUI_miniMemory()
    {
        miniMemoryUI.SetActive(true);
        fullMemoryUI.SetActive(false);
        blockMemoryUI.SetActive(false);
        switchUITypePage.SetActive(false);
    }

    public void setUI_blockMemory()
    {
        blockMemoryUI.SetActive(true);
        miniMemoryUI.SetActive(false);
        fullMemoryUI.SetActive(false);
        switchUITypePage.SetActive(false);
    }

    public void setUI_switchUITypePage()
    {
        switchUITypePage.SetActive(true);
        blockMemoryUI.SetActive(false);
        miniMemoryUI.SetActive(false);
        fullMemoryUI.SetActive(false);
    }

    #endregion

    #region BLOCK MEMORY UI FUNCTIONS
    
    public void initBlockMemoryUITable()
    {
        for(int i=0; i<10; i++) // for each row in Block Memory UI Table
        {
            for(int j=0; j<3; j++) // for each element in a row of Block Memory UI Table
            {
                string blockSetObjectName = "BlockSet (" + (i+1).ToString() + ")";
                string indexObjectName = "BlockIndex (" + (j+1).ToString() +")" + "/Index";
                string backgroundObjectName = "BlockIndex (" + (j+1).ToString() +")" + "/Panel";
                
                GameObject indexObject = blockMemoryUI.transform.Find("Content Table/" + blockSetObjectName + "/" + indexObjectName).gameObject;
                GameObject backgroundObject = blockMemoryUI.transform.Find("Content Table/" + blockSetObjectName + "/" + backgroundObjectName).gameObject;
                
                indexObject.GetComponent<TextMeshProUGUI>().text = String.Format("{0:00}", (j * 10 + i));
                backgroundObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void blockMemoryUITable_ADD_DATA(int blockNumber)
    {
        string blockSetObjectName = "BlockSet (" + (blockNumber % 10 + 1).ToString() + ")";
        string backgroundObjectName = "BlockIndex (" + (blockNumber / 10 + 1).ToString() +")" + "/Panel";

        GameObject backgroundObject = blockMemoryUI.transform.Find("Content Table/" + blockSetObjectName + "/" + backgroundObjectName).gameObject;
        backgroundObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f);
    }   

    public void blockMemoryUITable_ADD_INSTRUCTION(int blockNumber)
    {
        string blockSetObjectName = "BlockSet (" + (blockNumber % 10 + 1).ToString() + ")";
        string backgroundObjectName = "BlockIndex (" + (blockNumber / 10 + 1).ToString() +")" + "/Panel";
        
        GameObject backgroundObject = blockMemoryUI.transform.Find("Content Table/" + blockSetObjectName + "/" + backgroundObjectName).gameObject;
        backgroundObject.GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f);
    }   

    public void blockMemoryUITable_ADD_PAGE_TABLE(int blockNumber)
    {
        string blockSetObjectName = "BlockSet (" + (blockNumber % 10 + 1).ToString() + ")";
        string backgroundObjectName = "BlockIndex (" + (blockNumber / 10 + 1).ToString() +")" + "/Panel";
        
        GameObject backgroundObject = blockMemoryUI.transform.Find("Content Table/" + blockSetObjectName + "/" + backgroundObjectName).gameObject;
        backgroundObject.GetComponent<Image>().color = new Color(0.0f, 1.0f, 1.0f);
    }   

    #endregion

    #region FULL MEMORY UI FUNCTIONS

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
