using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [System.Serializable]
    public class TimedEvent
    {
        //executa uma funcao guardada
        public Callback Method;
        //tempo para executar o method
        public float TimeToExecute;

    }

    public List<TimedEvent> events;

    public delegate void Callback();
    private void Awake()
    {
        events = new List<TimedEvent>();
    }
    public void Add(Callback method, float inSeconds)
    {
        events.Add(new TimedEvent
        {
            Method = method,
            TimeToExecute = Time.time + inSeconds

        });
    }
    private void Update()
    {
        if (events.Count == 0)
            return;
        for (int i = 0; i < events.Count; i++)
        {
            var timedEvent = events[i];
            if (timedEvent.TimeToExecute <= Time.time)
            {
                timedEvent.Method();
                events.Remove(timedEvent);
            }
        }
    }
}