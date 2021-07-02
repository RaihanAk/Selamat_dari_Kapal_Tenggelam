using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSelectionUI : MonoBehaviour
{
    public Text titleCaseText;
    public Text descCaseText;
    public Image imageCase;

    public Button startCaseButton;

    private IntroManager introManager;

    private void Start()
    {
        introManager = GetComponent<IntroManager>();
    }

    public void UpdateCaseField(CaseData caseData)
    {
        titleCaseText.text = caseData.titleCase;
        descCaseText.text = caseData.descCase;
        imageCase.sprite = caseData.imageCase;

        startCaseButton.onClick.AddListener(() => introManager.OnClickCaseSelected(caseData.idCase));
    }
}
