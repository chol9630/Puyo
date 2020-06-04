using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesMager : MonoBehaviour
{

    public AudioClip StartSound;
    AudioSource AudioSource;

    public void ChageMainScene()
    {
        SceneManager.LoadScene("Tile");
    }
    public void ChangeMapChioceSecne()
    {
        SceneManager.LoadScene("MapSeletion");


    }

    public void ChangeGameScene()
    {
        SceneManager.LoadScene("MainGame");


    }

    public void Update()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.PlayOneShot(StartSound);
            }
        }
    }


    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }


    public void Quit()
    {
#if UNITY_EDITOR  //유니티 에디터에서 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else  //에디터가 아닌 PC,모바일 빌드 상태다

        Application.Quit();
#endif
    }






}
