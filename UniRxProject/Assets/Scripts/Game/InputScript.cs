using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputScript : MonoBehaviour
{
    public void InputScriptInit(UnityEvent event1, UnityEvent event2, UnityEvent event3, UnityEvent event4)
    {
        Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.UpArrow)).Subscribe(_ => event1.Invoke());
        Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.DownArrow)).Subscribe(_ => event2.Invoke());
        Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.RightArrow)).Subscribe(_ => event3.Invoke());
        Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.LeftArrow)).Subscribe(_ => event4.Invoke());
    
    }
}
