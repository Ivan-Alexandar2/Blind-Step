using System.Collections;
using UnityEngine;

public class EntryScene : MonoBehaviour
{
    public SceneFader fader;
    void Start()
    {
        fader = FindObjectOfType<SceneFader>();
        StartCoroutine(EntryCoroutine());
    }

    public IEnumerator EntryCoroutine()
    {
        yield return new WaitForSeconds(2);
        fader.FadeToScene(1);
    }
}
