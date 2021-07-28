using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PersonaCluster
{
    public GameObject person;
    public string name;
    public int age;
    [TextArea(3, 10)] public string description;
    public Transform oriPosition;
    public Transform camPostition;
    public int seatedAt;
    public Animator animasi;
}


[System.Serializable]
public class LifeBoat
{
    public GameObject seat;
    public string name = "";
}

public class Station2Manager : GameManager 
{
    [Header("Data pool")]
    [SerializeField] PersonaCluster[] personaClusters;
    [SerializeField] LifeBoat[] lifeBoats;

    public int[] personaSavedIndex;
    public Transform faceCamera;

    public bool isStationComplete = false;
    private float score = 0;

    public float timePassed = 0;
    public float dueTime = 480;

    public float delayFirstDialogue = 3;
    public float delayBackToLab = 5f;

    [Header("Audio SFX")]
    public AudioSource boatEngine;
    public AudioSource objSFX;

    [Header("Component")]
    public SceneChanger sceneChanger;
    public Station2UI station2UI;
    public DialogueManager dialogueManager;

    private PanelSequencer panelSequencer;

    public PersonaCluster tempCluster;

    #region UNITY CALLBACK
    private void Start()
    {
        panelSequencer = GetComponent<PanelSequencer>();

        // Assign method if the event is called. Assign using += remove using -=
        dialogueManager.OnDialogueEndConfirmation += DialogueTrigger;

        StartCoroutine(DelayStartDialogue());
    }

    // Fixed Update is called once per fixed time
    void FixedUpdate()
    {
        // Only run the timer IF instruction has completed AND Station has yet to complete
        if (station2UI.isInstructionComplete && !isStationComplete)
            timePassed += Time.deltaTime;
    }

    void Update()
    { 
        // Check if instruction for station 1 has completed yet
        if (station2UI.isInstructionComplete)
        {
            // Check if station 1 is not completed yet
            if (!isStationComplete)
            {
                // Check if all question have been answered OR reaching dueTime
                if (timePassed > dueTime)
                {
                    // EndGame
                    isStationComplete = true;
                    StationEnds();
                }
            }
        }
    }
    #endregion

    #region UI CALLBACKS
    public void SelectPersona(int codePersona)
    {
        objSFX.Play();

        tempCluster = personaClusters[codePersona];

        station2UI.namePersona.text = "Nama : " + personaClusters[codePersona].name;
        station2UI.agePersona.text = "Umur : " + personaClusters[codePersona].age;
        station2UI.descPersona.text = "Deskripsi : " + personaClusters[codePersona].description;

        UpdateFaceCamera();
    }

    public void SelectLifeBoat(int priority)
    {
        objSFX.Play();

        // Priority on editor are set to 1,2,3, or 4
        // priority--;

        // HARUS udah ada persona yg dipilih. Baru bisa milih LifeBoat seat
        if (station2UI.namePersona.text.Equals("Nama : "))
        {
            Debug.Log("Click any of the personas first");
        }
        else
        {
            int indexPersona = 0;



            /* Cases: If clicking to a seat but the seat's already taken by another persona. 
             * Solution: Move the persona on the seat to original position,
             *      then move currently selected personas to selected LifeBoat seat. 
            */
            if (! lifeBoats[priority].name.Equals(""))
            {
                // Cari index personanya sabaraha
                for (int i = 0; i < personaClusters.Length; i++)
                    if (lifeBoats[priority].name == personaClusters[i].name)
                        indexPersona = i;

                // Move the persona to the original position
                personaClusters[indexPersona].person.transform.position =
                    personaClusters[indexPersona].oriPosition.position;
                personaClusters[indexPersona].person.transform.rotation =
                    personaClusters[indexPersona].oriPosition.rotation;
                personaClusters[indexPersona].animasi.SetInteger("Kondisi", 0);

                // The persona are not seated anymore
                personaClusters[indexPersona].seatedAt = personaClusters.Length + 1;
            }


            // Cari index personanya sabaraha
            for (int i = 0; i < personaClusters.Length; i++)
                if (tempCluster.name == personaClusters[i].name)
                    indexPersona = i;



            /* Case: Persona already seated, and currently selected. But clicked to another seat
             * Solution: remove name from lifeBoats.name and seatedAt from personaClusters.seatedAt
            */
            Debug.Log("IF? left val " + (personaClusters[indexPersona].seatedAt) + " right val " + (personaClusters.Length + 1));
            if (personaClusters[indexPersona].seatedAt != personaClusters.Length + 1)
            {
                lifeBoats[personaClusters[indexPersona].seatedAt].name = "";
                personaClusters[indexPersona].seatedAt = personaClusters.Length + 1;
            }

            Debug.Log("indexPersona:" + indexPersona + " priority:" + priority);



            /* Standard function protocol
             * 
             */
            // Retrieve name info
            lifeBoats[priority].name = tempCluster.name;
            Debug.Log("name: " + lifeBoats[priority].name);

            // Pindahin persona ke lifeboat
            personaClusters[indexPersona].person.transform.position =
                lifeBoats[priority].seat.transform.position;
            personaClusters[indexPersona].person.transform.rotation =
                lifeBoats[priority].seat.transform.localRotation;

            // Now the persona are seated and update the camera
            personaClusters[indexPersona].seatedAt = priority;  //priority++;
            UpdateFaceCamera();
            Debug.Log(lifeBoats[priority].name +" seated at " + personaClusters[indexPersona].seatedAt);

            // Time to stop the wavy-wavy-please-notice-me animation
            // TO-DO: Switch the animator to idle-seating pose.
            personaClusters[indexPersona].animasi.SetInteger("Kondisi", 1);

        }
        
        // Update List
        UpdatePassengerInfo();
    }

    public void UpdatePassengerInfo()
    {
        string buffer = "";
        for (int i = 0; i < lifeBoats.Length; i++)
            buffer += i + 1 +". " +lifeBoats[i].name +"\n";

        station2UI.lifeboatText.text = buffer;
    }
    #endregion

    public void DialogueTrigger()
    {
        if (dialogueManager.dialogueInt == 5)
        {
            panelSequencer.StartPanelSequencer();
            objSFX.Play();

            StartCoroutine(DelayStartDialogue(2));
        }
        else if (dialogueManager.dialogueInt == 10)
            station2UI.isInstructionComplete = true;
    }

    public void CalculateScore()
    {
        float jawabanBenar = 0;

        // Get all the current persona on the lifeboat
        for (int i = 0; i < lifeBoats.Length; i++)
        {
            for (int j = 0; j < personaClusters.Length; j++)
            {
                if (lifeBoats[i].name.Equals(personaClusters[j].name))
                {
                    personaSavedIndex[i] = j;
                    break;
                }
            }
        }

        /* 
         * New formulae 
         */
        for (int i = 0; i < personaSavedIndex.Length; i++)
        {
            if (personaSavedIndex[i] == 4 ||
                personaSavedIndex[i] == 3 ||
                personaSavedIndex[i] == 2 ||
                personaSavedIndex[i] == 1)
                jawabanBenar++;
        }
        Debug.Log("Jawaban benar: " +jawabanBenar);


        /* Calculating */
        if (jawabanBenar == 4)
        {
            score = 90;

            if (personaSavedIndex[0] == 4 &&
                personaSavedIndex[1] == 3 &&
                personaSavedIndex[2] == 2 &&
                personaSavedIndex[3] == 1)
                score = 100;
        }
        else if (jawabanBenar == 3)
        {
            score = 80;
        }
        else if (jawabanBenar == 2)
        {
            score = 40;

            for (int i = 0; i < personaSavedIndex.Length; i++)
            {
                if (personaSavedIndex[i] == 4 ||
                    personaSavedIndex[i] == 3)
                        score += 10;
            }
        }
        else if (jawabanBenar == 1)
        {
            score = 20;
        }
        else
            score = 0;

        // score = tempScore;
        Debug.Log("Final score is : " + score);
    }

    private void UpdateFaceCamera()
    {
        faceCamera.position = tempCluster.camPostition.transform.position;
        faceCamera.rotation = tempCluster.camPostition.transform.rotation;
    }

    public override void ReportNewScore()
    {
        // Save score on main GameManager 
        if (ProgressCache.Instance != null)
            ProgressCache.Instance.ReportNewValue(score);
    }

    public void StationEnds()
    {
        isStationComplete = true;

        // Finish Dialogue
        dialogueManager.StartPlayDialogue();
        boatEngine.Play();

        // Calculate score based on whoever picked to be on the boat at the time
        CalculateScore();
        Debug.Log("Station 2 End. Score: " + score + ". Time left: " + (dueTime - timePassed));
        station2UI.ShowScore(score);

        ReportNewScore();        

        // Ends
        StartCoroutine(DelayBackToLab(delayBackToLab));
    }

    public float GetRemainingTime()
    {
        return dueTime - timePassed;
    }

    private IEnumerator DelayStartDialogue()
    {
        yield return new WaitForSeconds(delayFirstDialogue);

        dialogueManager.StartPlayDialogue();
    }

    private IEnumerator DelayStartDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);

        dialogueManager.StartPlayDialogue();
    }

    private IEnumerator DelayBackToLab(float duration)
    {
        yield return new WaitForSeconds(duration);

        StartCoroutine(sceneChanger.LoadSceneAsync("Ending"));
    }
}
