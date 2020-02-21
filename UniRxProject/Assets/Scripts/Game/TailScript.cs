using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UniRx;
using UniRx.Triggers;
public class TailScript : MonoBehaviour
{
    public GameObject target;
    CompositeDisposable disposables;
    public float FollowDistance = 2f;
    ReactiveProperty<float> distance = new ReactiveProperty<float>();
    private void OnEnable()
    {
        disposables =new CompositeDisposable();

        transform.UpdateAsObservable().Subscribe((_) =>
        {
            distance.Value = Vector3.Distance(transform.position, target.transform.position);
        }).AddTo(disposables);

        var stream = distance.AsObservable().Where(y =>
            y > FollowDistance)
                .Subscribe((x) =>
                {
                    transform.LookAt(target.transform.position);
                    transform.position += transform.forward * Time.deltaTime*distance.Value*3;
                }).AddTo(disposables);
    }
    public void DisposeDisposables()
    {
        disposables.Dispose();
    }
}
