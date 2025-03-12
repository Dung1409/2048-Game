using System;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public static Dictionary<string , List<Action>> Listener = new Dictionary<string, List<Action>>();
    public static void AddListener(string name, Action callback)
    {
        if(!Listener.ContainsKey(name)) 
        {
            Listener[name] = new List<Action>();    
        }
        Listener[name].Add(callback); 
    }
    public static void RemoveListener(string name , Action callback) 
    {
        if(Listener.ContainsKey(name)) 
        {
            return;
        }
        Listener[name].Remove(callback);    
    }
    public static void Notify(string name) 
    {
        if (!Listener.ContainsKey(name))
        {
            return;
        }
        foreach(Action callback in Listener[name]) 
        {
            try
            {
                callback?.Invoke();
            }catch(Exception e) { 
                Debug.LogException(e);
            }
        }
    }
}
