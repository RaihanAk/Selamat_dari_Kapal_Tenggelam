using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Runs dialogue. Takes data from DialogueScrObject
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public DialogueScrObject[] dialogues;

    [Tooltip("If later subtitle text were implemented")]
    public Text dialogueTextScreen;

    private int dialogueInt = 0;

    [Header("Component")]
    [Tooltip("Speakers to play the dialogue may be > 1")]
    public AudioSource[] speakers;


    /// <summary>
    /// OnCountdownTimerHasExpired delegate.
    /// </summary>
    public delegate void DialogueEndConfirmation();
    /// <summary>
    /// Called when the timer has expired.
    /// </summary>
    public event DialogueEndConfirmation OnDialogueEndConfirmation;

    /// <summary>
    /// Start playing dialogue sequentially
    /// </summary>
    public void StartPlayDialogue()
    {
        StartCoroutine(StartDialogue());
    }

    /// <summary>
    /// Start playing dialogue number n listed on Dialogue Manager GameObject
    /// </summary>
    /// <param name="dialogueNum">dialogue number to be played</param>
    public void StartPlayDialogue(int dialogueNum)
    {
        StartCoroutine(StartDialogue(dialogueNum));
    }

    /// <summary>
    /// Start playing dialogue with code listed on Dialogue Manager GameObject
    /// </summary>
    /// <param name="dialogueNum">dialogue code to be played</param>
    public void StartPlayDialogue(string dialogueCode)
    {
        StartCoroutine(StartDialogue(dialogueCode));
    }

    private IEnumerator StartDialogue()
    {
        // Plays and display subtitle if available
        for(int i = 0; i < speakers.Length; i++)
        {
            speakers[i].PlayOneShot(dialogues[dialogueInt].clip);
        }
        if (dialogueTextScreen != null)
            dialogueTextScreen.text = dialogues[dialogueInt].dialogueText;

        // Wait till audio done playing
        while (speakers[0].isPlaying)
        {
            yield return null;
        }

        // Empty subtitle text
        if (dialogueTextScreen != null)
            dialogueTextScreen.text = "";

        // Decides what to do after a dialogue has ended
        switch (dialogues[dialogueInt].afterDelayType)
        {
            case DialogueScrObject.AfterDelayType.Confirmation:
                dialogueInt++;

                // Trigger event to be called
                if (OnDialogueEndConfirmation != null)
                    OnDialogueEndConfirmation();

                break;

            case DialogueScrObject.AfterDelayType.Seconds:
                yield return new WaitForSeconds(dialogues[dialogueInt].waitSeconds);
                dialogueInt++;

                StartCoroutine(StartDialogue());

                break;

            case DialogueScrObject.AfterDelayType.End:
                dialogueInt++;

                break;
        }
    }

    private IEnumerator StartDialogue(int dialogueNum)
    {
        // Plays and display subtitle if available
        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].PlayOneShot(dialogues[dialogueNum].clip);
        }
        if (dialogueTextScreen != null)
            dialogueTextScreen.text = dialogues[dialogueNum].dialogueText;

        // Wait till audio done playing
        while (speakers[0].isPlaying)
        {
            yield return null;
        }

        // Empty subtitle text
        if (dialogueTextScreen != null)
            dialogueTextScreen.text = "";
    }

    private IEnumerator StartDialogue(string dialogueCode)
    {
        int dialogueNum = -1;

        for (int i = 0; i < dialogues.Length; i++)
        {
            if (dialogues[i].name == dialogueCode)
                dialogueNum = i;
        }

        if (dialogueNum == -1)
            Debug.LogError("Dialogue code not found");
        else
        {
            // Plays and display subtitle if available
            for (int i = 0; i < speakers.Length; i++)
            {
                speakers[i].PlayOneShot(dialogues[dialogueNum].clip);
            }
            if (dialogueTextScreen != null)
                dialogueTextScreen.text = dialogues[dialogueNum].dialogueText;

            // Wait till audio done playing
            while (speakers[0].isPlaying)
            {
                yield return null;
            }

            // Empty subtitle text
            if (dialogueTextScreen != null)
                dialogueTextScreen.text = "";
        }
    }
}
