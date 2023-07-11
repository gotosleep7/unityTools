using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class Loom : MonoBehaviour
{
    //Is it initialized
    static bool isInitialized;

    private static Loom _ins;
    public static Loom ins { get { Initialize(); return _ins; } }

    void Awake()
    {
        _ins = this;
        isInitialized = true;
    }

    //initialization
    public static void Initialize()
    {
        if (!isInitialized)
        {
            if (!Application.isPlaying)
                return;

            isInitialized = true;
            var obj = new GameObject("Loom");
            _ins = obj.AddComponent<Loom>();

            DontDestroyOnLoad(obj);
        }
    }

    //Single execution unit (no delay)
    struct NoDelayedQueueItem
    {
        public Action<object> action;
        public object param;
    }
    //All execution list (no delay)
    List<NoDelayedQueueItem> listNoDelayActions = new List<NoDelayedQueueItem>();


    //Single execution unit (with delay)
    struct DelayedQueueItem
    {
        public Action<object> action;
        public object param;
        public float time;
    }
    //All execution list (with delay)
    List<DelayedQueueItem> listDelayedActions = new List<DelayedQueueItem>();


    //Join the main thread execution queue (no delay)
    public static void QueueOnMainThread(Action<object> taction, object param)
    {
        QueueOnMainThread(taction, param, 0f);
    }

    //Join the main thread execution queue (with delay)
    public static void QueueOnMainThread(Action<object> action, object param, float time)
    {
        if (time != 0)
        {
            lock (ins.listDelayedActions)
            {
                ins.listDelayedActions.Add(new DelayedQueueItem { time = Time.time + time, action = action, param = param });
            }
        }
        else
        {
            lock (ins.listNoDelayActions)
            {
                ins.listNoDelayActions.Add(new NoDelayedQueueItem { action = action, param = param });
            }
        }
    }


    //Currently executing function chain without delay
    List<NoDelayedQueueItem> currentActions = new List<NoDelayedQueueItem>();
    //The currently executing function chain with delay
    List<DelayedQueueItem> currentDelayed = new List<DelayedQueueItem>();

    void Update()
    {
        if (listNoDelayActions.Count > 0)
        {
            lock (listNoDelayActions)
            {
                currentActions.Clear();
                currentActions.AddRange(listNoDelayActions);
                listNoDelayActions.Clear();
            }
            for (int i = 0; i < currentActions.Count; i++)
            {
                currentActions[i].action(currentActions[i].param);
            }
        }

        if (listDelayedActions.Count > 0)
        {
            lock (listDelayedActions)
            {
                currentDelayed.Clear();
                currentDelayed.AddRange(listDelayedActions.Where(d => Time.time >= d.time));
                for (int i = 0; i < currentDelayed.Count; i++)
                {
                    listDelayedActions.Remove(currentDelayed[i]);
                }
            }

            for (int i = 0; i < currentDelayed.Count; i++)
            {
                currentDelayed[i].action(currentDelayed[i].param);
            }
        }
    }

    void OnDisable()
    {
        if (_ins == this)
        {
            _ins = null;
        }
    }
}