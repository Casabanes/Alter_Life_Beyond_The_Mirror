using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public delegate void Callback(params object[] parameters);

    public static EventManager instance;

    private Dictionary<string, Callback> _events = new Dictionary<string, Callback>();

    public event Action<bool> pause;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }
    public void Suscribe(string eventid, Callback callback)
    {
        if (!_events.ContainsKey(eventid))
        {
            _events.Add(eventid, callback);
        }
        else
        {
            _events[eventid] += callback;
        }
    }
   public void TutorialPauseGame(bool value)
    {
        pause?.Invoke(value);
    }
    public void Unsuscribed(string eventid, Callback callback)
    {
        if (_events.ContainsKey(eventid))
            _events[eventid] -= callback;
    }

    public void Trigger(string eventid, params object[] parameters)
    {
        if (_events.ContainsKey(eventid))
            _events[eventid](parameters);
    }
}
