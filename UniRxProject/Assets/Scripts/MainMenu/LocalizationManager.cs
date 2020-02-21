using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx.Triggers;
using UniRx;
using System.Linq;
using UniRx.Operators;

public class LocalizationManager : MonoBehaviour
{
    public enum UIText
    {
        StartGame, Quit, HighScore, Title, TitlePlayerNamePrefix, LoseScreen, InputFieldTitleText, Points, Restart, ToMainMenu
    }
    public enum Localization
    {
        EN, FI
    }

    public ReactiveProperty<Localization> CurrentLocalization = new ReactiveProperty<Localization>();
    public List<ReactiveProperty<string>> LocalizationTexts = new List<ReactiveProperty<string>>();
    public string TitlePlayerNamePrefix;
    [SerializeField]
    public Sprite ENFlag, FIFlag;
    Button LanguageButton;
    private void Awake()
    {
        CurrentLocalization.Value = Localization.EN;
    }
    public void SetMainMenuLocalizations(Button Start, Button Quit, Button Highscore, Button Language,
        TextMeshProUGUI Title, Text PlayerNameInputField, TextMeshProUGUI InputFieldTitleText,TextMeshProUGUI HighScoreWindowTitle, EndScreenHandler loseScreen)
    {
        this.ObserveEveryValueChanged(x => CurrentLocalization.Value).Subscribe(x =>
        {
            GetLocalization();
            if (x == Localization.EN) Language.image.sprite = FIFlag; else Language.image.sprite = ENFlag;
        });
        LocalizationTexts[(int)UIText.StartGame].SubscribeToText(Start.GetComponentInChildren<Text>());
        LocalizationTexts[(int)UIText.Quit].SubscribeToText(Quit.GetComponentInChildren<Text>());
        LocalizationTexts[(int)UIText.HighScore].SubscribeToText(Highscore.GetComponentInChildren<Text>());
        LocalizationTexts[(int)UIText.HighScore].Subscribe(x => HighScoreWindowTitle.text = x);
        LocalizationTexts[(int)UIText.Title].Subscribe(x => Title.text = x);
        LocalizationTexts[(int)UIText.InputFieldTitleText].Subscribe(x => InputFieldTitleText.text = x);
        LanguageButton = Language;
        LocalizationTexts[(int)UIText.LoseScreen].Subscribe(x => loseScreen.TitleText.text = x);
        LocalizationTexts[(int)UIText.Restart].Subscribe(x => loseScreen.RestartButton.GetComponentInChildren<Text>().text = x);
        LocalizationTexts[(int)UIText.ToMainMenu].Subscribe(x => loseScreen.BackToMenuButton.GetComponentInChildren<Text>().text = x);


        

    }
    public void SwitchLocalization()
    {
        if (CurrentLocalization.Value == Localization.EN)
        {
            CurrentLocalization.Value = Localization.FI; LanguageButton.image.sprite = ENFlag;
        }
        else CurrentLocalization.Value = Localization.EN; LanguageButton.image.sprite = FIFlag;
    }

    public void GetLocalization()
    {
        var reader = new StreamReader($"{Application.dataPath}/Localizations/Localization{CurrentLocalization.ToString()}.txt");
        var filecontents = reader.ReadToEnd();
        reader.Close();
        var array = filecontents.Split("\n"[0]);
        for (int i = 0; i < array.Length; i++)
        {
            LocalizationTexts.Add(new ReactiveProperty<string>());
            LocalizationTexts[i].Value = array[i];
        }
        TitlePlayerNamePrefix = LocalizationTexts[(int)UIText.TitlePlayerNamePrefix].Value;
    }
}
