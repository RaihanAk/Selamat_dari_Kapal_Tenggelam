using UnityEngine;
using UnityEngine.UI;

public class CountdownPanelController : MonoBehaviour
{
    public Station2Manager gameManager;
    public Text countdownText;

    public bool isShowing = false;

    void FixedUpdate()
    {
        string tempTimer = string.Format("{0:00}", gameManager.GetRemainingTime());
        countdownText.text = tempTimer;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(!isShowing);

        isShowing = !isShowing;
    }
}
