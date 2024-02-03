using System;
using System.Collections.Generic;
using UnityEngine;

public static class UnityThread
{
    private static readonly List<Action> actions = new List<Action>();
    private static bool initialized = false;

    public static void Init()
    {
        if (!initialized)
        {
            // Crea un GameObject y adjúntale este script para ejecutar las acciones en el hilo principal
            var executor = new GameObject("UnityThreadExecutor").AddComponent<UnityThreadExecutor>();
            GameObject.DontDestroyOnLoad(executor.gameObject);
            initialized = true;
        }
    }

    public static void ExecuteInUpdate(Action action)
    {
        lock (actions)
        {
            actions.Add(action);
        }
    }

    public class UnityThreadExecutor : MonoBehaviour
    {
        void Update()
        {
            List<Action> actionsToExecute = new List<Action>();
            lock (actions)
            {
                actionsToExecute.AddRange(actions);
                actions.Clear();
            }

            foreach (var action in actionsToExecute)
            {
                action();
            }
        }
    }
}
