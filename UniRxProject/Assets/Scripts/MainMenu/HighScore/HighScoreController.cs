using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    Pooling Pool;
    List<GameObject> ScoreObjects = new List<GameObject>();
    public static UnityEvent OnHighScoreChanged = new UnityEvent();
    List<KeyValuePair<string, int>> HighScores = new List<KeyValuePair<string, int>>();
    HighScoreManager ScoreManager = new HighScoreManager();
    [SerializeField] Transform ScoreParent;
    [SerializeField] Button ClearScoresButton;

    void Start()
    {
        HighScores = ScoreManager.LoadHighScores();
        foreach(var obj in HighScores)
        {
            Debug.Log("Name: " + obj.Key + " Score: " + obj.Value);
        }
        Pool = GetComponent<Pooling>();
        OnHighScoreChanged.AddListener(RefreshHighScoreList);
        ClearScoresButton.onClick.AddListener(ScoreManager.ClearScores);
        gameObject.SetActive(false);
    }
    private void RefreshHighScoreList()
    {
        HighScores = ScoreManager.HighScores;
        if (HighScores.Count > 1) { 
            HighScores.Sort((hs1, hs2) => hs1.Value.CompareTo(hs2.Value));
        }
        foreach (var obj in ScoreObjects)
        {
            Pool.ReturnToPool(obj);
        }
        ScoreObjects.Clear();

        foreach(var score in HighScores)
        {
            var obj = Pool.FromPool();
            obj.transform.SetParent(ScoreParent);
            var ScoreObj = obj.GetComponent<HighScoreScript>();
            ScoreObj.InitScore(score);
            ScoreObjects.Add(obj);
        }
    }
    public void AddScore(string name, int points)
    {
        ScoreManager.AddHighScore(new KeyValuePair<string, int>(name, points));
    }
    private void OnApplicationQuit()
    {
        ScoreManager.SaveHighScores(HighScores, () => Debug.Log("Quit Game"));
    }
    private void OnEnable()
    {
        RefreshHighScoreList();
    }
}
