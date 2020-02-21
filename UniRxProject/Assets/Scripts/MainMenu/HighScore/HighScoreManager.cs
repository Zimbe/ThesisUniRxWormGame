using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager
{
    public List<KeyValuePair<string, int>> HighScores = new List<KeyValuePair<string, int>>();
    
    public void SaveHighScores(List<KeyValuePair<string, int>> highscores, System.Action action)
    {
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < HighScores.Count; i++)
        {
            PlayerPrefs.SetString($"HighScoreName{i}", highscores[i].Key);
            PlayerPrefs.SetInt($"HighScore{i}", highscores[i].Value);
        }
        if(action != null) action.Invoke();
    }
    public List<KeyValuePair<string, int>> LoadHighScores()
    {
        HighScores = new List<KeyValuePair<string, int>>();
        for (int i = 0; i < HighScores.Count; i++)
        {
            HighScores.Add(new KeyValuePair<string, int>($"HighScoreName{i}", PlayerPrefs.GetInt($"HighScore{i}")));
        }
        return HighScores;
    }
    public void AddHighScore(KeyValuePair<string, int> scorePair)
    {
        HighScores.Add(scorePair);
    }
    public void ClearScores()
    {
        HighScores.Clear();
        PlayerPrefs.DeleteAll();
    }
}
 