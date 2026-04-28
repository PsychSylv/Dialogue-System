using System.Collections.Generic;
using UnityEngine;

public class DialogCSVController : MonoBehaviour
{
    //Tujuan script ini untuk extract data dialog dr csv, lalu disimpan dalam bentuk dict

    [SerializeField] private TextAsset dialogCSV;

    private Dictionary<string, DialogData> dialogDict = new Dictionary<string, DialogData>();

    public void LoadDialogData()
    {
        string[] dialogLines = dialogCSV.text.Split("\n");

        //Debug.Log(dialogLines[1]);

        for(int i = 1; i < dialogLines.Length; i++) //Hindari Header
        {
            string line = dialogLines[i].Trim();
            //Debug.Log(line); //Hasil: D001,"""Ad astra abyssosque! Welcome to the Adventurers' Guild.""","""Who Are You?"";""Claim Reward""",D002;D005

            //Dibagi dulu, Parsing
            string[] dialogParts = ParseDialogData(line);

            DialogData dialogData = new DialogData();
            
            dialogData.dialogID = dialogParts[0];
            dialogData.dialogText = dialogParts[1];

            //Debug.Log(dialogParts[1]);

            dialogData.hasChoice = !string.IsNullOrEmpty(dialogParts[2]);

            if (!string.IsNullOrEmpty(dialogParts[2]))
            {
                dialogData.dialogChoices = dialogParts[2].Split(';');
            }
               
            if(!string.IsNullOrEmpty(dialogParts[3]))
            {
                dialogData.nextDialogID = dialogParts[3].Split(';');
            }



            dialogDict[dialogData.dialogID] = dialogData; //Masukin ke Dict
        }
    }

    public DialogData GetDialogData(string id)
    {
        return dialogDict.ContainsKey(id) ? dialogDict[id] : null; //Kalo g ada balikin null
    }

    private string[] ParseDialogData(string line)
    {
        List<string> result = new List<string>();
        bool inQuote = false; //Check dia dalam "" ga
        string current = ""; //String skrg kosong dulu isiny

        foreach (char c in line)
        {
            if(c == '"') //Di dalam " "
            {
                inQuote = !inQuote; //Dibikin kebalikanny
            }
            else if(c == ',' && !inQuote) //Artiny , yang memisah, bukan dalam quote
            {
                result.Add(current);
                current = ""; //Hapus!
            }
            else
            {
                current += c;
            }            
        }

        result.Add(current);
        return result.ToArray(); //Bikin jd array
    }

}
