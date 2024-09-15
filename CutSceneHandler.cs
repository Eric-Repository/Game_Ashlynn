using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutSceneHandler : MonoBehaviour
{
    public GameObject cutscene1;
    public GameObject am1;
    public GameObject am2;
    public Image ri1;
    VideoPlayer video;
    public AudioSource audio;
    public Text skipText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) )
        {
            SkipVideo();
        }

    }

    private void Awake()
    {
        video = cutscene1.transform.GetChild(0).GetComponent<VideoPlayer>();
    }

    public void PlayVideo()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(FadeImage());
        
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
        
    }

    public void SkipVideo()
    {
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
        
    }



    IEnumerator FadeImage()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            ri1.color = new Color(1, 1, 1, i);
            yield return null;

        }
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            audio.volume = i * 0.125f; ;
            yield return null;
        }

        //yield return new WaitForSeconds(1f);
        StartCoroutine(playVideo());

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            ri1.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, i);
            yield return null;
        }


    }

    IEnumerator playVideo()
    {
        cutscene1.SetActive(true);
        //audio.volume = 0f;
        video.Play();
        am1.SetActive(false);
        am2.SetActive(false);
        video.loopPointReached += CheckOver;
        yield return null;

    }
}
