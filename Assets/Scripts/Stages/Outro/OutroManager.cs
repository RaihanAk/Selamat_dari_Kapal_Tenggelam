using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutroManager : MonoBehaviour
{
    public Text scoreLabelText;
    public Text scoreText;

    [Header("Dialogue")]
    public float delayFirstDialogue = 3f;
    public DialogueManager dialogueManager;
    public bool isReadyPlayDialogue = true;

    [Header("Component")]
    public AudioSource audioSFX;
    public SceneChanger sceneChanger;

    private void Start()
    {
        // Assign method if the event is called. Assign using += remove using -=
        dialogueManager.OnDialogueEndConfirmation += InstructionEnd;

        if (ProgressCache.Instance != null)
        {
            scoreLabelText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            scoreText.text = ProgressCache.Instance.resultValue[0].ToString();
            
            //ProgressCache.Instance.SaveData();
        }

        if (isReadyPlayDialogue)
            StartCoroutine(DelayStartDialogue());
    }

    public void InstructionEnd()
    {
        scoreLabelText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        
        dialogueManager.StartPlayDialogue();
    }

    private IEnumerator DelayStartDialogue()
    {
        yield return new WaitForSeconds(delayFirstDialogue);

        dialogueManager.StartPlayDialogue();
    }
}
