using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public VictoryManager victoryManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            victoryManager.TriggerVictory();
        }
    }
}
