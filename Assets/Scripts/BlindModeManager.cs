using UnityEngine;
using UnityEngine.UI;

public class BlindModeManager : MonoBehaviour
{
    [Header("UI References")]
    public Image blackOverlay;

    void Start()
    {
        // Get the saved choice from the Main Menu. 
        // The '0' at the end is a fallback default just in case nothing was saved.
        int isBlindMode = PlayerPrefs.GetInt("BlindMode", 0);

        // If the player chose Blind Mode (1), turn the black screen on. 
        // Otherwise, turn it off.
        if (isBlindMode == 1)
        {
            blackOverlay.enabled = true;
            Debug.Log("Playing in BLIND MODE");
        }
        else
        {
            blackOverlay.enabled = false;
            Debug.Log("Playing in CLASSIC MODE");
        }
    }
}
