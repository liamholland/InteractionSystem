using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newConversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    //This is the information contained in a Conversation object
    public ConversationLine[] conversationLines;
    public bool availableAtStart; //The inspector variable which is transfered to isAvailable when the game starts

    [HideInInspector] public bool isAvailable = false; //This is the variable to change if you want to
                                                       //make a collectable make this conversation
                                                       //available
}
