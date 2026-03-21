using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;

    [Header("Settings")]
    public float fadeSpeed = 0.5f; // Higher number = faster fade

    void Start()
    {
        GameObject fadeImageObject = GameObject.Find("FadingImage");
        fadeImage = fadeImageObject.GetComponent<Image>();
        // Every time a scene loads with this script, it automatically fades IN
        StartCoroutine(FadeIn());
    }

    // This is the public function your buttons will call
    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOutAndLoad(sceneIndex));
    }

    // --- THE COROUTINE FORMULAS ---

    private IEnumerator FadeIn()
    {
        // 1. Start completely black
        Color fadeColor = fadeImage.color;
        fadeColor.a = 1f; // 'a' stands for Alpha (1 is solid, 0 is invisible)
        fadeImage.color = fadeColor;

        // 2. Slowly subtract from the Alpha until it reaches 0
        while (fadeImage.color.a > 0f)
        {
            fadeColor.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = fadeColor;
            yield return null; // "Wait for the next frame before looping again"
        }

        // 3. Turn off the raycast shield so the player can click things again
        fadeImage.raycastTarget = false;
    }

    private IEnumerator FadeOutAndLoad(int sceneIndex)
    {
        // 1. Turn on the raycast shield so they can't click other buttons
        fadeImage.raycastTarget = true;

        Color fadeColor = fadeImage.color;

        // 2. Slowly add to the Alpha until it reaches 1 (completely black)
        while (fadeImage.color.a < 1f)
        {
            fadeColor.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = fadeColor;
            yield return null;
        }

        // 3. The screen is now 100% black. Load the next scene!
        SceneManager.LoadScene(sceneIndex);
    }
}
