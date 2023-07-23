using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndFadeToBlack : MonoBehaviour
{
    [SerializeField] private int _layerPlayer;
    [SerializeField] private Image[] _image;
    [SerializeField] private float _distanceToShowSelfEntirely;
    [SerializeField] private float _distanceToEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            foreach (var item in _image)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b
            , Mathf.Lerp(1, 0, (Vector3.Distance(transform.position, GameManager.instance._currentCharacter.transform.position)
             - 3 - _distanceToShowSelfEntirely) / _distanceToShowSelfEntirely));
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            foreach (var item in _image)
            {

                item.color = new Color(item.color.r, item.color.g, item.color.b
          , Mathf.Lerp(1, 0, (Vector3.Distance(transform.position, GameManager.instance._currentCharacter.transform.position)
           - 3 - _distanceToShowSelfEntirely) / _distanceToShowSelfEntirely));

            }
            if(Vector3.Distance(transform.position,GameManager.instance.GetPlayer())< _distanceToEnd)
            {
                EventManager.instance.TutorialPauseGame(true);
                GameManager.instance.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GameManager.instance.gameObject.GetComponent<Rigidbody>().isKinematic=true;
                SceneManager.LoadScene("0");
                foreach (var item in _image)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, 1);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            foreach (var item in _image)
            {

                item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
            }
        }
    }
}
