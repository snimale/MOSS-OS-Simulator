using System.Text;
using UnityEngine;

public class InputFileObjectScript : MonoBehaviour
{
    private string inputFileContent = null;
    private int inputFileLinePointer = 0; // points to next to read line
    private int inputFileCharPointer = 0; // points to next to read character

    private void OnEnable() 
    {
        initializeInputFileContent();
    }

    private void initializeInputFileContent()
    {
        string fileName = PlayerPrefs.GetString("input_file_name");
        inputFileContent = FileHandler.loadFile(fileName);
        inputFileLinePointer = 0;
        inputFileCharPointer = 0;
    }

    public bool isFileLoaded()
    {
        if(inputFileContent == null)
            return true;
        return false;
    }

    #region get/set functions

    public void setInputFileContent(string text) 
    {
        inputFileContent = text;
    }

    public string getInputFileContent()
    {
        return inputFileContent;
    }

    public string getNextInputLine()
    {
        // no input to read
        if(inputFileCharPointer >= inputFileContent.Length)
            return "\0";

        // some input to read
        StringBuilder nextLine = new StringBuilder();   
        char currentCharacter = inputFileContent[inputFileCharPointer];

        // read input
        while(inputFileCharPointer < inputFileContent.Length)
        {
            if(inputFileContent[inputFileCharPointer] == '\0' || inputFileContent[inputFileCharPointer] == '\n')
                break;
            else
            {
                nextLine.Append(inputFileContent[inputFileCharPointer]);
                inputFileCharPointer++;
            }
        }

        // post-read changes
        inputFileCharPointer++;
        inputFileLinePointer++;
        nextLine.Append('\0');

        return nextLine.ToString();
    }

    #endregion
}
