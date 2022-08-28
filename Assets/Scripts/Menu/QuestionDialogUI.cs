using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialogUI : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private Button yesBtn;
    private Button noBtn;

    public static QuestionDialogUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        yesBtn = transform.Find("YesBtn").GetComponent<Button>();
        noBtn = transform.Find("NoBtn").GetComponent<Button>();
        Hide();
    }

    public void ShowQuestion(string question, Action Yes, Action No)
    {
        gameObject.SetActive(true);
        textMeshPro.text = question;
        yesBtn.onClick.AddListener(()=> {
            Hide();
            Yes();
        });
        noBtn.onClick.AddListener(()=> {
            Hide();
            No();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
