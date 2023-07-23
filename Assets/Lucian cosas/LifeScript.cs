using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public UnityEngine.UI.Image vida;


    void Start()
    {
        EventManager.instance.Suscribe("PlayerDamage", PlayerDamage);
        EventManager.instance.Suscribe("PlayerDamage2", PlayerDamage2);
        vida = gameObject.GetComponent<UnityEngine.UI.Image>();
    }


    private void PlayerDamage(params object[] parameters)
    {
        vida.fillAmount = (float)parameters[0];
    }

    private void PlayerDamage2(params object[] parameters)
    {
        vida.fillAmount -= (float)parameters[0];
    }
}
