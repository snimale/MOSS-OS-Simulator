using UnityEngine;
using UnityEngine.SceneManagement;

public class MachineUIController : MonoBehaviour
{
    public void OnClick_BACK()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
