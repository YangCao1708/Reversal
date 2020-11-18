using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Image BlackScreen;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReturnToMain());
    }


    IEnumerator ReturnToMain()
    {
        yield return new WaitForSecondsRealtime(39.5f);
        BlackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(2f);
        GameManager.Instance.StartGame(0);
    }
}
