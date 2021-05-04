using UnityEngine;

/// <summary>
/// The gameobject will be destroyed if found new gameobject with AudioSingleton attached. Tailor-made for bgm.
/// </summary>
public class AudioSingleton : MonoBehaviour
{
    public static AudioSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance.gameObject.name != this.gameObject.name)
            {
                // Anjay reverse singletod
                Destroy(Instance.gameObject);

                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}
