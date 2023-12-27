using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Animator anim;

    public void Play()
    {
        StartCoroutine(NextLevel());
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator NextLevel()
    {
        anim.SetTrigger("start");

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
