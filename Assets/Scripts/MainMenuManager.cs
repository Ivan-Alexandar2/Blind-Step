using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource pressButtonAudio;
    public Image fadeImage;
    public float fadeSpeed = 1f;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);

        GameObject pressBtnAudio = GameObject.Find("ButtonAudio");
        pressButtonAudio = pressBtnAudio.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartClassicMode();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Application.Quit();
        }
    }
    public void StartClassicMode()
    {
        StartCoroutine(PlayCoroutine());
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

    public IEnumerator PlayCoroutine()
    {
        pressButtonAudio.Play();

        // 1. Turn on the raycast shield so they can't click other buttons
        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        Color fadeColor = fadeImage.color;

        // 2. Slowly add to the Alpha until it reaches 1 (completely black)
        while (fadeImage.color.a < 1f)
        {
            fadeColor.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = fadeColor;
            yield return null;
        }

        yield return new WaitForSeconds(3);

        // Save the player's choice (0 means Classic, 1 means Blind)
        PlayerPrefs.SetInt("BlindMode", 0);
        PlayerPrefs.Save();

        // Load the Tutorial or First Level (Scene index 1)
        SceneManager.LoadScene(1);
    }
}
