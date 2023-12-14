using UnityEngine;

public class CreateOutputUI : MonoBehaviour
{
    private IOFile ioFile;
    
    void OnEnable()
    {
        ioFile.Name = null;
        ioFile.content = null;    
    }

    
}
