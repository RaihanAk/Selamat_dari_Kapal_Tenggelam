using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Runs all dialogue. Takes data from DialogueScrObject
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public DialogueScrObject[] dialogues;
    public Text dialogueTextScreen;

    private int dialogueInt = 0;

    [Header("Component")]
    public AudioSource[] speakers;  // Speakers to play the dialogue may be > 1

    public IEnumerator StartDialogue()
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
}
