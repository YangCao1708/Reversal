using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Animator BlackScreen;
    public Text EndWords;

    public float TransitionSpeed;

    public void TransitionToGame()
    {
        StartCoroutine(GameTransition());
    }

    public void TransitionToMain()
    {
        StartCoroutine(MenuTransition());
    }

    public void TransitionToCredits()
    {
        StartCoroutine(CreditsTransition());
    }

    public void FadeIn()
    {
        BlackScreen.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        BlackScreen.SetTrigger("FadeOut");
    }

    public void FadeInWords()
    {
        StartCoroutine(FadeInText());
    }

    IEnumerator GameTransition()
    {
        FadeIn();
        yield return new WaitForSecondsRealtime(TransitionSpeed);
        GameManager.Instance.StartGame(GameManager.Instance.GetLevelInGame());
    }

    IEnumerator MenuTransition()
    {
        FadeIn();
        yield return new WaitForSecondsRealtime(TransitionSpeed);
        GameManager.Instance.SaveLevel();
        GameManager.Instance.StartGame(0);
    }

    IEnumerator CreditsTransition()
    {
        FadeIn();
        yield return new WaitForSecondsRealtime(TransitionSpeed);
        GameManager.Instance.SaveLevel();
        SceneManager.LoadScene(2);
        SoundManager.Instance.PlayCreditsBGM();
    }

    IEnumerator FadeInText()
    {
        EndWords.gameObject.SetActive(true);
        while (EndWords.color.a < 1)
        {
            EndWords.color = new Color(EndWords.color.r, EndWords.color.g, EndWords.color.b, EndWords.color.a + 0.05f);
            yield return null;
        }
    }
}
