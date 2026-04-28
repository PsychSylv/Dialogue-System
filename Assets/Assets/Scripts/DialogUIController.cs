using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Transform choiceHolder;

    [SerializeField] private GameObject choiceCanvas;

    public event Action<string> onChoiceClicked;

    public void ShowText(string text) //Ubah text dialog
    {
        dialogText.text = text;
    }

    public void ShowChoice(DialogData dialogData) //Tunjukin choice
    {
        ClearChoice();
        choiceCanvas.SetActive(true);

        string[] dialogChoices = dialogData.dialogChoices;

        for (int i = 0; i < dialogChoices.Length; i++)
        {
            int index = i;

            GameObject dialogButton = Instantiate(choiceButtonPrefab, choiceHolder); //Instantiate button baru
            dialogButton.GetComponentInChildren<TextMeshProUGUI>().text = dialogChoices[i]; //Ubah text choice button sesuai dialog data

            //Set Event
            dialogButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                onChoiceClicked(dialogData.nextDialogID[index]); //Set event
            });
        }
    }

    public void DisableChoice()
    {
        choiceCanvas.SetActive(false);
    }

    private void ClearChoice()
    {
        foreach(Transform button in choiceHolder)
        {
            Destroy(button.gameObject);
        }
    }
}
