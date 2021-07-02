using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station2UI : MonoBehaviour
{
    public Station2Manager station2Manager;

    public bool isInstructionComplete = false;

    //kumpulan text
    [Header("Essential Text")]
    public Text countdownText;

    [Header("Persona's information")]
    public Text namePersona;
    public Text agePersona,
        descPersona,
        lifeboatText;

    [Header("Persona's information")]
    public Text debugText;

    private int TutorialText = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInstructionComplete)
        {
            float res = station2Manager.dueTime - station2Manager.timePassed;

            string tempTimer = string.Format("{0:00}", res);
            countdownText.text = tempTimer;
        }
    }

    public void ShowScore(float score)
    {

        //txtKeteranganCara.text = ("Score anda ") + score;

        if (debugText != null)
            debugText.text = "score at text: " + score.ToString();
        Debug.Log("score at text: " + score.ToString());

        countdownText.text = score.ToString();
    }
}
