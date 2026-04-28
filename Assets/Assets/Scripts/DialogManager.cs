using UnityEngine;

public enum GameState
{
    None,
    Dialog,
    Choice
}

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogCSVController dialogCSVController;
    [SerializeField] private DialogUIController dialogUIController;

    private GameState gameState;

    private DialogData currentDialogData;
    private bool endOfDialog;

    private void Start()
    {
        dialogUIController.onChoiceClicked += ShowDialog;

        gameState = GameState.None;

        dialogCSVController.LoadDialogData(); //Load data dialog ke dalam dict

        //Lgsg Load Dialog Pertama!
        dialogUIController.gameObject.SetActive(true);
        ShowDialog("D001");
    }

    private void OnDestroy()
    {
        dialogUIController.onChoiceClicked -= ShowDialog;
    }

    private void ShowDialog(string dialogID)
    {
        currentDialogData = dialogCSVController.GetDialogData(dialogID);

        //Debug.Log(currentDialogData.dialogText);

        if(!currentDialogData.hasChoice) //Kalau g ada choice, disable choice menu, show text saja
        {
            gameState = GameState.Dialog;

            dialogUIController.DisableChoice();
            dialogUIController.ShowText(currentDialogData.dialogText);
        }
        else //Kalau ada choice, show text, nyalakan choice menu
        {
            gameState = GameState.Choice;

            dialogUIController.ShowText(currentDialogData.dialogText);
            dialogUIController.ShowChoice(currentDialogData);
        }

        if(currentDialogData.nextDialogID == null) //Cek kalo sudah di dialog terakhir
        {
            endOfDialog = true;
        }
        else
        {
            endOfDialog = false;
        }
    }

    private void Update()
    {
        //Lanjutin Dialog = tinggal click
        if(Input.GetKeyDown(KeyCode.Mouse0) && gameState == GameState.Dialog)
        {
            //Matikan dialog kalau sudah akhir
            if (endOfDialog) dialogUIController.gameObject.SetActive(false);
            else
            {
                //Move To Next Dialog
                ShowDialog(currentDialogData.nextDialogID[0]); //Kalo dialog biasa pasti cuma ada 1 nextDialogID
            }            
        }
    }
}
