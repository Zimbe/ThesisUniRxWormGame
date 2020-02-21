using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UniRx;
using UniRx.Triggers;

public class MenuController : MonoBehaviour
{
    LocalizationManager LocalizationManager;
    [SerializeField]
    GameController GameController;

    //UI fields
    [SerializeField]
    GameObject HighScoreWindow;
    [SerializeField]
    Button StartButton;
    [SerializeField]
    Button QuitButton;
    [SerializeField]
    Button HighScoreButton;
    [SerializeField]
    Button LanguageButton;
    [SerializeField]
    TMP_InputField PlayerNameInputField;
    [SerializeField]
    TextMeshProUGUI TitleText;
    [SerializeField]
    Text PlayerNameTitleText;
    [SerializeField]
    TextMeshProUGUI InputFieldTitleText;
    [SerializeField]
    TextMeshProUGUI HighScoreWindowTitle;
    [SerializeField]
    EndScreenHandler LoseScreen;
    [SerializeField]
    TextMeshProUGUI Points;
    //

    string PlayerName;
    public static IReactiveProperty<int> PlayerPoints = new ReactiveProperty<int>();
    LocalizationManager.Localization DefaultLocalization = LocalizationManager.Localization.EN;

    CompositeDisposable disposables;

    public static UnityEvent LoseEvent = new UnityEvent();
    public static UnityEvent BackToMainMenuEvent = new UnityEvent();
    [SerializeField] HighScoreController ScoreController;

    void Start()
    {
        LocalizationManager = GetComponent<LocalizationManager>();
        LocalizationManager.SetMainMenuLocalizations(StartButton, QuitButton, HighScoreButton, LanguageButton, TitleText, PlayerNameTitleText, InputFieldTitleText, HighScoreWindowTitle, LoseScreen);
        MainMenuSubscribtions();

        PlayerPoints.Subscribe(_ =>Points.text = _.ToString());
        GameController.PointsEvent.AddListener(()=> PlayerPoints.Value++);
        BackToMainMenuEvent.AddListener(()=> UIController.ChangeUIState.Invoke());
        HighScoreWindow.SetActive(false);
        LoseScreen.Initialize(()=>GameController.StartGame(), UIController.ChangeUIState.Invoke);
        LoseEvent.AddListener(()=> {
            LoseScreen.Activate(PlayerName,GetPoints() );
            ScoreController.AddScore(PlayerName, PlayerPoints.Value);
            PlayerPoints.Value = 0;
        });
    }
    int GetPoints()
    {
        return PlayerPoints.Value;
    }
    void MainMenuSubscribtions()
    {
        StartButton.onClick.AsObservable().Subscribe(_ =>
        {
            UIController.ChangeUIState.Invoke();
            GameController.StartGame();
        });
        QuitButton.onClick.AsObservable().Subscribe(_ => Application.Quit());
        HighScoreButton.onClick.AsObservable().Subscribe(_ => ToggleHighScore());
        LanguageButton.onClick.AsObservable().Subscribe(_ =>
        {
            LocalizationManager.SwitchLocalization();
            PlayerNameTitleText.text = "";
        });
        PlayerNameInputField.onSelect.AsObservable().Subscribe(_ => InputFieldStreams());
    }

    void InputFieldStreams()
    {
        disposables = new CompositeDisposable();
        PlayerNameInputField.onValueChanged.AsObservable().Select(_ => PlayerNameInputField.text)
            .Subscribe(_ => PlayerNameTitleText.text = $"{ LocalizationManager.TitlePlayerNamePrefix} {_}");

        PlayerNameInputField.onEndEdit.AsObservable().Select(x => PlayerNameInputField.text).Subscribe(x =>
        {
            PlayerName = x;
            PlayerNameInputField.text = "";
            disposables.Dispose();
        }).AddTo(disposables);
    }
    void ToggleHighScore()
    {
        if (HighScoreWindow.activeInHierarchy) HighScoreWindow.SetActive(false); else HighScoreWindow.SetActive(true);
    }
}
