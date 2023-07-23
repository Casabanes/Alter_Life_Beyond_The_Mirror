using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private GameObject[] _menuItems;
    [SerializeField] private float _videoDuration;
    private Action _updateDelegate;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update()
    {
        _updateDelegate?.Invoke();
    }
    public void PlayGame()
    {
        foreach (var item in _menuItems)
        {
            item.SetActive(false);
        }
        _video.Play();
        _updateDelegate += SkipVideo;
        StartCoroutine(ChangeSceneWhenVideoEnds());
    }
    private void SkipVideo()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    private IEnumerator ChangeSceneWhenVideoEnds()
    {
        yield return new WaitForSeconds(1);
        _updateDelegate += ChangeScene;
    }
    private void ChangeScene()
    {
        if (!_video.isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
