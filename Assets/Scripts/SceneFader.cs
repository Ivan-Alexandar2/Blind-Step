using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;

    [Header("Settings")]
    public float fadeSpeed = 3f;

    void Start()
    {
        GameObject fadeImageObject = GameObject.Find("FadingImage");
        if (fadeImageObject != null) fadeImage = fadeImageObject.GetComponent<Image>();

        StartCoroutine(FadeIn());
    }

    // Call this for Restart or Next Level
    public void FadeToScene(int sceneIndex)
    {
        // We pass the "LoadScene" command as a callback
        StartCoroutine(FadeOut(() => SceneManager.LoadScene(sceneIndex)));
    }

    // Call this for Quitting
    public void FadeToQuit()
    {
        // We pass the "Quit" command as a callback
        StartCoroutine(FadeOut(() => Application.Quit()));
    }

    // --- THE COROUTINES ---

    private IEnumerator FadeIn()
    {
        Color fadeColor = fadeImage.color;
        fadeColor.a = 1f;
        fadeImage.color = fadeColor;

        while (fadeImage.color.a > 0f)
        {
            // USING UNSCALED TIME so it works even when Time.timeScale is 0!
            fadeColor.a -= Time.unscaledDeltaTime * fadeSpeed;
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeImage.raycastTarget = false;
    }

    // Notice the 'Action onComplete' parameter
    private IEnumerator FadeOut(Action onComplete)
    {
        fadeImage.raycastTarget = true;
        Color fadeColor = fadeImage.color;

        while (fadeImage.color.a < 1f)
        {
            // USING UNSCALED TIME
            fadeColor.a += Time.unscaledDeltaTime * fadeSpeed;
            fadeImage.color = fadeColor;
            yield return null;
        }

        // Unfreeze the game right before we load the new scene or quit
        Time.timeScale = 1f;

        // Execute whatever action we passed in!
        onComplete?.Invoke();
    }

    void AAAAAA() { }
}