using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UtilitiesManager : MonoBehaviour
{
    private bool paused;
    [SerializeField] private GameObject _quit;
    private void Start()
    {
        if (_quit == null)
        {
            Debug.Log("Error, quit is not assigned");
            Destroy(gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.instance.TutorialPauseGame(!_quit.activeSelf);
            _quit.SetActive(!_quit.activeSelf);
            if (!_quit.activeSelf)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
