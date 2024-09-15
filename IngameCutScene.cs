using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IngameCutScene : MonoBehaviour
{
    #region Singleton

    public static IngameCutScene instance;

    private void Awake()
    {
        instance = this;
        dinoVideo = cutscene2.transform.GetChild(0).GetComponent<VideoPlayer>();
        underwaterVideo = doplhinCutscene.transform.GetChild(0).GetComponent<VideoPlayer>();
        flyingVideo = birdCutscene.transform.GetChild(0).GetComponent<VideoPlayer>();

    }
    #endregion



    public GameObject cutscene2;
    public GameObject doplhinCutscene;
    public GameObject birdCutscene;

    public Image ri1;
    VideoPlayer dinoVideo;
    VideoPlayer underwaterVideo;
    VideoPlayer flyingVideo;
    public AudioSource audio;
    public Text skipText;
    public bool enterDinoworld = false;
    public bool enterUnderWaterworld = false;
    public bool enterFloatingIslandworld = false;

    public bool isVideoPlaying = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isVideoPlaying)
        {
            SkipVideo();
        }

    }

   
    public void PlayVideo()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isVideoPlaying = true;

        if (enterDinoworld)
        {
            StartCoroutine(FadeImage(dinoVideo, cutscene2));
        }

        if (enterUnderWaterworld)
        {
            StartCoroutine(FadeImage(underwaterVideo, doplhinCutscene));
        }

        if (enterFloatingIslandworld)
        {
            StartCoroutine(FadeImage(flyingVideo, birdCutscene));

        }



    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        isVideoPlaying = false;
        //fade out the video
        if (enterDinoworld)
        {
            cutscene2.SetActive(false);
        }

        if (enterUnderWaterworld)
        {
            doplhinCutscene.SetActive(false);
        }

        if (enterFloatingIslandworld)
        {
            birdCutscene.SetActive(false);
        }

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            ri1.color = new Color(1, 1, 1, i);
        }

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            audio.volume = i * 0.125f; ;
        }
    }

    public void SkipVideo()
    {
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ri1.color = new Color(1, 1, 1, 0);

        if (enterDinoworld)
        {
            cutscene2.SetActive(false);
        }

        if (enterUnderWaterworld)
        {
            doplhinCutscene.SetActive(false);
        }

        if (enterFloatingIslandworld)
        {
            birdCutscene.SetActive(false);
        }
        //SceneManager.LoadScene(1);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            audio.volume = i * 0.125f; ;
        }

        isVideoPlaying = false;
    }



    IEnumerator FadeImage(VideoPlayer video, GameObject cutsceneOBJ)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            ri1.color = new Color(1, 1, 1, i);

        }

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            audio.volume = i * 0.125f; ;
        }

        StartCoroutine(playVideo(video, cutsceneOBJ));

        

        yield return new WaitForSeconds(5f);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, i);
            yield return null;
        }


    }

    IEnumerator playVideo(VideoPlayer v, GameObject cutSceneOBJ)
    {
        cutSceneOBJ.SetActive(true);
        //audio.volume = 0f;
        v.Play();   
        v.loopPointReached += CheckOver;
        yield return null;

    }
}
