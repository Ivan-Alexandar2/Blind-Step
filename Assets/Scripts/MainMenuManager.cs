using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // This will be triggered by the Classic Mode button
    public void StartClassicMode()
    {
        // Save the player's choice (0 means Classic, 1 means Blind)
        PlayerPrefs.SetInt("BlindMode", 0);
        PlayerPrefs.Save();

        // Load the Tutorial or First Level (Scene index 1)
        SceneManager.LoadScene(1);
    }

    // This will be triggered by the Blind Mode button
    public void StartBlindMode()
    {
        // Save the choice as 1 (Blind)
        PlayerPrefs.SetInt("BlindMode", 1);
        PlayerPrefs.Save();

        // Load the same level!
        SceneManager.LoadScene(1);
    }
}
