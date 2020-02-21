using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UniRx;
using UniRx.Triggers;
using System;

public class EndScreenHandler : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TitleText;
    [SerializeField] public TextMeshProUGUI PointsText;
    [SerializeField] public Button RestartButton;
    [SerializeField] public Button BackToMenuButton;
    public string pointsPrefix;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Initialize(Action restartAction, Action menuAction)
    {
        RestartButton.onClick.AddListener(()=>
        {
            restartAction.Invoke();
            gameObject.SetActive(false);
        });
        BackToMenuButton.onClick.AddListener(()=> { 
            menuAction.Invoke();
            gameObject.SetActive(false);
        });
    }
    public void Activate(string name, int points)
    {
        gameObject.SetActive(true);
        PointsText.text = pointsPrefix+points.ToString();
    }

}
