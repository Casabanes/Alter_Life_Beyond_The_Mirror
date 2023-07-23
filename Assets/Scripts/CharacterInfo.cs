using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private GameObject _jakeInfo;
    [SerializeField] private GameObject _tudorInfo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch(GameManager.instance._currentCharacter._character)
            {
                case (BaseCharacter.Identity.jake):
                    if (_jakeInfo.activeSelf)
                    {
                        _jakeInfo.SetActive(false);
                        EventManager.instance.TutorialPauseGame(false);
                    }
                    else
                    {
                        EventManager.instance.TutorialPauseGame(true);
                        _jakeInfo.SetActive(true);
                    }
                    break;
                case (BaseCharacter.Identity.Tudor):
                    if (_tudorInfo.activeSelf)
                    {
                        EventManager.instance.TutorialPauseGame(false);
                        _tudorInfo.SetActive(false);
                    }
                    else
                    {
                        EventManager.instance.TutorialPauseGame(true);
                        _tudorInfo.SetActive(true);
                    }
                    break;
            }
        }
    }
}
