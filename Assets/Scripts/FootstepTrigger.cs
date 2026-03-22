using UnityEngine;

public class FootstepTrigger : MonoBehaviour
{
    public Echolocation echolocationScript;

    // The animation timeline will call this exact function!
    public void FireFootstep()
    {
        Debug.Log("AAAAAAAA");
        if (echolocationScript != null)
        {
            echolocationScript.FirePulse();
        }
    }
}
