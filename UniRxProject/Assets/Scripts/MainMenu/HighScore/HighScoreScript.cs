using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI PointsText;
    public void InitScore(KeyValuePair<string, int> scorePair)
    {
        NameText.text = scorePair.Key;
        PointsText.text = scorePair.Value.ToString();
    }
}
