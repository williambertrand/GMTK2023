using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    public int currentScore;
    [SerializeField] private TMP_Text scoreText;
    
    void Start()
    {
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHumanCaught(Human h)
    {
        // TODO: Just counting right now, but if we want to do points we can update that here
        currentScore += h.scoreValue;
        scoreText.text = string.Format("{0} Humans Caught", currentScore);
    }
}
