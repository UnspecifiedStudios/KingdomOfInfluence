using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour {
    private static UnityMainThreadDispatcher instance;
    private readonly Queue<Action> actions = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance() {
        if (instance == null) {
            Debug.LogError("UnityMainThreadDispatcher instance not found in scene!");
        }
        return instance;
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        lock (actions) {
            while (actions.Count > 0)
                actions.Dequeue().Invoke();
        }
    }

    public void Enqueue(Action action) {
        lock (actions) {
            actions.Enqueue(action);
        }
    }
}
