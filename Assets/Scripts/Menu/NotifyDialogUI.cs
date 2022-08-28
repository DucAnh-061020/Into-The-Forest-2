using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NotifyDialogUI : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public static NotifyDialogUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Hide();
    }

    public void ShowDialog(string text)
    {
        gameObject.SetActive(true);
        textMeshPro.text = text;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
