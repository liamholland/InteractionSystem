using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
    public CanvasGroup dialogueBoxContents;
    public Conversation defaultConversation; //If no other conversations are available, this coversation will play
    public Conversation[] conversations; //All the conversations this Npc can have
    public Text speakerName;
    public Text speakerDialogue;
    public Animator animator; //Animator for dialouge box
    public RectTransform dialogueBox;

    private Conversation nextConversation;
    private ConversationLine nextConversationLine;
    private InteractableObject npcController;
    private int currentLineNum;
    private bool speaking = false;
    private float dialogueBoxHeight;

    //Gets the Npc script and checks if the conversations relating to
    // the Npc are available at the start of the game
    private void Start()
    {
        npcController = GetComponent<InteractableObject>();
        foreach(Conversation conversation in conversations)
        {
            if (conversation.availableAtStart)
            {
                conversation.isAvailable = true;
            }
        }
    }

    //constantly checks if the dialougue box content should be shown.
    //If set to 100 bugs can occur where the height is set to around
    // 99.9996 which stops the info from showing
    private void Update()
    {
        dialogueBoxHeight = dialogueBox.rect.height;
        if (dialogueBoxHeight >= 98)
            dialogueBoxContents.alpha = 1;
        else
            dialogueBoxContents.alpha = 0;
    }

    //Decides what conversation should happen
    private Conversation UpdateConversation()
    {
        foreach (Conversation conversation in conversations)
        {
            if (conversation.isAvailable)
            {
                return conversation;
            }
        }
        return defaultConversation;
    }

    //Starts the Conversation. This starts the animation as well
    public void StartConversation()
    {
        animator.SetBool("show", true);
        nextConversation = UpdateConversation();
        currentLineNum = 0;
        speaking = true;
        if (nextConversation != null)
        {
            UpdateDialogueBox();
        }
    }

    //Ends the conversation
    public void EndConversation()
    {
        animator.SetBool("show", false);
        nextConversation = UpdateConversation();
        npcController.isTalking = false;
    }

    //Changes the lines displayed in the conversation. If all the lines have been used
    //it ends the conversation
    public void UpdateDialogueBox()
    {
        if (speaking)
        {
            nextConversationLine = nextConversation.conversationLines[currentLineNum];
            speakerName.text = nextConversationLine.speakerName;
            speakerDialogue.text = nextConversationLine.speakerDialog;
        }
        else
        {
            nextConversation.isAvailable = false;
            EndConversation();
        }
    }

    //Updates the index of the line array in the current conversation then
    //calls UpdateDialogueBox() to display it. If the next index is greater
    //than the lines in the conversation is changes speaking to false;
    public void UpdateLine()
    {
        currentLineNum++;
        if(currentLineNum > nextConversation.conversationLines.Length - 1)
        {
            speaking = false;
        }
        UpdateDialogueBox();
    }
}
