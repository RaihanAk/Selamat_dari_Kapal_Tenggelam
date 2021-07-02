using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.Controllables;

public class RescueBoatLeverInfo : MonoBehaviour
{
    public GameObject machineSteam;

    [Header("Component")]
    public Station2Manager station2Manager;
    public VRTK_BaseControllable controllable;
    
    public Text displayText;
    public string onText = "MAX";
    public string offText = "MIN";

    private VRTK_InteractableObject io;

    protected virtual void OnEnable()
    {
        if (controllable != null)
        {
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
        }
        Invoke("SetupIOListeners", 0.1f);
    }

    protected virtual void OnDisable()
    {
        if (controllable != null)
        {
            controllable.MaxLimitReached -= MaxLimitReached;
            controllable.MinLimitReached -= MinLimitReached;
        }

        if (io != null)
        {
            io.InteractableObjectTouched -= InteractableObjectTouched;
        }
    }

    protected virtual void SetupIOListeners()
    {
        io = controllable.GetComponentInParent<VRTK_InteractableObject>();
        if (io != null)
        {
            io.InteractableObjectTouched += InteractableObjectTouched;
        }
    }

    protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
    {
        UpdateText(offText);
    }

    protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
    {
        UpdateText(onText);
        if (station2Manager != null)
            FinishLevel();
    }

    protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        // Do something when grabbed
    }

    protected virtual void UpdateText(string text)
    {
        if (displayText != null)
        {
            displayText.text = text;
        }
    }

    private void FinishLevel()
    {
        machineSteam.SetActive(true);
        station2Manager.StationEnds();
    }
}
