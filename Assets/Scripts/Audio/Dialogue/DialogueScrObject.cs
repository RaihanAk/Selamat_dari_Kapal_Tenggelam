using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stores dialogue information
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Dialogue/New Dialogue Data")]
public class DialogueScrObject : ScriptableObject
{
    public string dialogueName;

    public AudioClip clip;
    public string dialogueText;

    public enum AfterDelayType
    {
        Seconds,
        Confirmation,
        End
    }
    [Tooltip("What to do after this dialogue ends")]
    public AfterDelayType afterDelayType;

    public float waitSeconds;
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueScrObject)), CanEditMultipleObjects]
public class DialogueEditor : Editor
{
    public SerializedProperty afterDelayType_prop;

    private void OnEnable()
    {
        afterDelayType_prop = serializedObject.FindProperty("afterDelayType");
    }

    public override void OnInspectorGUI()
    {
        var DialogueGUI = target as DialogueScrObject;
        serializedObject.Update();

        EditorGUILayout.LabelField("Main Info");

        DialogueGUI.dialogueName = EditorGUILayout.TextField("Dialogue Name ", DialogueGUI.dialogueName);

        EditorGUILayout.PrefixLabel("Clip ");
        DialogueGUI.clip = (AudioClip)EditorGUILayout.ObjectField(DialogueGUI.clip, typeof(AudioClip), true,
            GUILayout.Height(EditorGUIUtility.singleLineHeight));

        EditorGUILayout.PrefixLabel("Dialogue Text ");
        DialogueGUI.dialogueText = EditorGUILayout.TextArea(DialogueGUI.dialogueText,
            GUILayout.ExpandHeight(true));


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dialogue Ends Action");

        EditorGUILayout.PropertyField(afterDelayType_prop);
        switch (DialogueGUI.afterDelayType)
        {
            case DialogueScrObject.AfterDelayType.Seconds:
                DialogueGUI.waitSeconds = EditorGUILayout.FloatField("Wait Seconds ", DialogueGUI.waitSeconds);
                break;
        }

        EditorUtility.SetDirty(DialogueGUI);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
