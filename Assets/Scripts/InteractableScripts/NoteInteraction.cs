using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteInteraction : BaseInteraction
{
    public PlayerPaperScript playerPaper;
    public GameObject noteUI;
    public GameObject NoteNotif;
    public TextMeshProUGUI NoteText;
    public string noteMessage = "";
    public override void Interact()
    {
        NoteText.text = noteMessage;
        NoteNotif.SetActive(true);
        noteUI.SetActive(true);
        playerPaper.ActivateHand(true);
    }
}
