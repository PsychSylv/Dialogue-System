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

        dialogCSVController.LoadDialogData();

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

        if(!currentDialogData.hasChoice)
        {
            gameState = GameState.Dialog;

            dialogUIController.DisableChoice();
            dialogUIController.ShowText(currentDialogData.dialogText);
        }
        else
        {
            gameState = GameState.Choice;

            dialogUIController.ShowText(currentDialogData.dialogText);
            dialogUIController.ShowChoice(currentDialogData);
        }

        if(currentDialogData.nextDialogID == null)
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
            if (endOfDialog) dialogUIController.gameObject.SetActive(false);
            else
            {
                //Move To Next Dialog
                ShowDialog(currentDialogData.nextDialogID[0]); //Kalo dialog biasa pasti cuma ada 1 nextDialogID
            }            
        }
    }
}
