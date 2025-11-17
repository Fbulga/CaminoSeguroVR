using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] public TMP_Text touchStreetText;
    [SerializeField] public TMP_Text dontLookSidesText;
    [SerializeField] public TMP_Text crossPedestrianText;
    void Start()
    {
        scoreText.text = GameManager.Instance.totalScore.ToString();
        touchStreetText.text = GameManager.Instance.touchStreetCount.ToString();
        dontLookSidesText.text = GameManager.Instance.dontLookSidesCount.ToString();
        crossPedestrianText.text = GameManager.Instance.crossPedestrianCount.ToString();
    }

}
