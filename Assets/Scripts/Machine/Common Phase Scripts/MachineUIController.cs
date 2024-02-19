using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MachineUIController : MonoBehaviour
{
    private bool isPaused; // true when user clicks pause

    private void Awake() {
        isPaused = false;
    }


    public void OnClick_BACK()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClick_PAUSE()
    {
        TextMeshProUGUI text = transform.Find("Pause/text").GetComponent<TextMeshProUGUI>();
        if(isPaused)
        {
            isPaused = false;
            text.text = "PAUSE";
        } else
        {
            isPaused = true;
            text.text = "RESUME";
        }
    }

    public bool get_isPaused() {
        return isPaused;
    }
}
