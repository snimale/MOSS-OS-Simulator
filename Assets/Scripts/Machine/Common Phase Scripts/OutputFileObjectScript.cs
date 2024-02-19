using System.Text;
using UnityEngine;

public class OutputFileObjectScript : MonoBehaviour
{
    private string originalOutputFileContent = null;
    private StringBuilder newOutputFileContent = null;
    private int outputFileCharPointer = 0; // always point to last char index + 1 of outputFileContent
    private int outputFileLinePointer = 0;

    private void OnEnable() 
    {
        initializeOriginalOutputFileContent();
        initializeNewOutputFileContent();
    }

    private void initializeOriginalOutputFileContent()
    {
        // get output file contents
        string fileName = PlayerPrefs.GetString("output_file_name", "default");
        originalOutputFileContent = FileHandler.loadFile(fileName);
        outputFileCharPointer = 0;
        outputFileLinePointer = 0;

        // get initial char and line ptr for outputFile
        while(outputFileCharPointer < originalOutputFileContent.Length)
        {
            if(originalOutputFileContent[outputFileCharPointer] == '\0')
                break;
            if(originalOutputFileContent[outputFileCharPointer] == '\n')
                outputFileLinePointer++;
            outputFileCharPointer++;
        }

        // post-content char
        outputFileCharPointer++;
        outputFileLinePointer++;
    }

    public void initializeNewOutputFileContent()
    {
        newOutputFileContent = new StringBuilder();
    }

    public void appendTextInNewOutput(string text)
    {
        newOutputFileContent = newOutputFileContent.Append(text);
    }

    public void writeBackNewOutputFileContent()
    {
        // write back in append mode
        string fileName = PlayerPrefs.GetString("output_file_name");   
        FileHandler.appendInFile(fileName, newOutputFileContent.ToString());
    }
}
