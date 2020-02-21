using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject MainMenuUI;
    public static UnityEvent ChangeUIState = new UnityEvent();

    void Start()
    {
        ChangeUIState.AddListener(() =>
        {
            if (MainMenuUI.activeInHierarchy)
            {
                MainMenuUI.SetActive(false);
            }
            else
            {
                MainMenuUI.SetActive(true);
            }
        });
    }
}
