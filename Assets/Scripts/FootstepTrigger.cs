using UnityEngine;

public class FootstepTrigger : MonoBehaviour
{
    public Echolocation echolocationScript;

    // The animation timeline will call this exact function!
    public void FireFootstep()
    {
        if (echolocationScript != null)
        {
            echolocationScript.FirePulse();
        }
    }
}
