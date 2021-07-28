using System;
using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("Case")]
    public CaseData[] cases;

    [Header("Dialogue")]
    public float delayFirstDialogue = 3f;
    public DialogueManager dialogueManager;
    public bool isReadyPlayDialogue = true;

    [Header("Transition Anim")]
    public GameObject[] fadingWalls;

    public GameObject toCaseOne;
    public float delayBeforeLoad = 1;

    [Header("Component")]
    public AudioSource audioSFX;
    public SceneChanger sceneChanger;

    private PanelSequencer panelSequencer;

    #region UNITY CALLBACK
    private void Start()
    {
        panelSequencer = GetComponent<PanelSequencer>();

        // Assign method if the event is called. Assign using += remove using -=
        dialogueManager.OnDialogueEndConfirmation += InstructionEnd;

        if (isReadyPlayDialogue)
            StartPlayDialogue();    
    }
    #endregion

    #region UI CALLBACK
    public void OnClickCaseSelected(int caseNumber)
    {
        switch (caseNumber)
        {
            case 0:
                StartCoroutine(TransitionToCaseOne());
                break;
            default:
                Debug.LogError("Case selection returns undefined case number");
                break;
        }
    }
    #endregion

    public void InstructionEnd()
    {
        panelSequencer.StartPanelSequencer();
        audioSFX.Play();
    }

    public void StartFadeWall()
    {
        for (int i = 0; i < fadingWalls.Length; i++)
        {
            StartCoroutine(FadeAlphaObj(fadingWalls[i]));
        }
    }

    private void StartPlayDialogue()
    {
        StartCoroutine(DelayStartDialogue());
    }

    private IEnumerator DelayStartDialogue()
    {
        yield return new WaitForSeconds(delayFirstDialogue);

        dialogueManager.StartPlayDialogue();
    }

    private IEnumerator TransitionToCaseOne()
    {
        // flood the room
        toCaseOne.SetActive(true);
        float time = delayBeforeLoad;
        Vector3 targetPos = new Vector3(
            toCaseOne.transform.position.x,
            (float)(toCaseOne.transform.position.y + 1.6),
            toCaseOne.transform.position.z);

        // Anim wall, transparent
        StartFadeWall();

        // Anim water, rise
        while (time > 0)
        {
            toCaseOne.transform.position =
                Vector3.Lerp(toCaseOne.transform.position, targetPos, .5f * Time.deltaTime);

            time -= Time.deltaTime;
            yield return null;
        }
        
        //yield return new WaitForSeconds(delayBeforeLoad);
        Debug.Log("End of animation, going to case 1...");

        StartCoroutine(sceneChanger.LoadSceneAsync("Station 2"));
    }

    private IEnumerator FadeAlphaObj(GameObject target)
    {
        Color ori = new Color(0, 0, 0);
        try
        {
            ori = target.GetComponent<MeshRenderer>().material.color;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        Color faded = new Color(0, 0, 0, 0);
        float fadeStart = 0;

        while (fadeStart < delayBeforeLoad)
        {
            fadeStart += Time.deltaTime * delayBeforeLoad;

            target.GetComponent<MeshRenderer>().material.color =
                Color.Lerp(ori, faded, fadeStart);
            yield return null;
        }
    }
}
