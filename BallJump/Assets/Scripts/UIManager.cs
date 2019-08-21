using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text bestText;
   
    void Update()
    {
        bestText.text = "Best: " + GameManager.singleton.best;
        scoreText.text = "Score: " + GameManager.singleton.score;
    }
}
