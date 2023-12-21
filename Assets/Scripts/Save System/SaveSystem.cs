using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private static readonly string SAVE_INPUTS_FOLDER = Application.dataPath + "/Saves/Inputs";
    private static readonly string SAVE_OUTPUTS_FOLDER = Application.dataPath + "/Saves/Outputs";

    public static void init()
    {
        // create directory if not exist
        if(!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);
        if(!Directory.Exists(SAVE_INPUTS_FOLDER))
            Directory.CreateDirectory(SAVE_INPUTS_FOLDER);
        if(!Directory.Exists(SAVE_OUTPUTS_FOLDER))
            Directory.CreateDirectory(SAVE_OUTPUTS_FOLDER);
    }

    public static void saveFile(string fileName, string text)
    {
        File.WriteAllText(SAVE_FOLDER + "/" + fileName, text);
    }

    public static string loadFile(string fileName)
    {
        string text = null;

        if (File.Exists(SAVE_FOLDER + "/" + fileName))
            text = File.ReadAllText(SAVE_FOLDER + "/" + fileName);
        else
            Debug.Log("File Not Found!");

        return text;
    }

    public static bool deleteFile(string fileName) 
    {
        if(File.Exists(SAVE_FOLDER + "/" + fileName))
        {
            File.Delete(SAVE_FOLDER + "/" + fileName);
            return true;
        }

        return false;
    }
}
