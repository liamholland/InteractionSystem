using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public InteractableType interactableType;
    public GameObject player;
    public string defaultText;
    public string altText;
    public bool hideInstructions;
    public Conversation[] conversationsToMakeAvailable;
    public string notificationToGive;

    [HideInInspector] public string interactionText; //The actual instructions displayed
    [HideInInspector] public bool isTalking = false; // The bool for Conversations. Must be Public
                                                     //due to use in the ConversationManager class

    private bool isBeingCarried = false; // The bool for CarryItem
    private bool hasBeenCollected = false; // The bool for CollectionItem

    private bool defaultHideInstructions; //If true, "Press E to" will no longer appear along with the
                                          //name of the object

    private void Start()
    {
        interactionText = defaultText;
        defaultHideInstructions = hideInstructions;
    }

    //Update calls the appropriate function for the item this script is attactched to,
    //but that fuction cannot do anything unless its variable is set to true by
    //DoInteraction()
    private void Update()
    {
        switch (interactableType)
        {
            case InteractableType.CarryItem:
                CarryItem();
                break;
            case InteractableType.CollectionItem:
                CollectionItem();
                break;
            case InteractableType.Conversation:
                Conversation();
                break;
        }
    }

    //Function for Conversation objects
    private void Conversation()
    {
        if (isTalking)
        {
            interactionText = "";
            hideInstructions = true;
            player.GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            interactionText = defaultText;
            hideInstructions = defaultHideInstructions;
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    //Function for Collectable Objects
    private void CollectionItem()
    {
        if (hasBeenCollected)
        {
            Destroy(gameObject);
        }
    }

    //Function for Carry Objects
    private void CarryItem()
    {
        if (isBeingCarried)
        {
            interactionText = altText;
            transform.position = player.transform.position;
        }
        else
        {
            interactionText = defaultText;
        }
    }

    //sets the appropriate variable to true on Interaction
    public void DoInteraction()
    {
        switch (interactableType)
        {
            case InteractableType.CarryItem:
                isBeingCarried = !isBeingCarried;
                break;
            case InteractableType.CollectionItem:
                hasBeenCollected = true;
                break;
            case InteractableType.Conversation:
                isTalking = true;
                GetComponent<ConversationManager>().StartConversation();
                break;
            default:
                Debug.Log("Could Not Match Interactable Type");
                break;
        }

        if(conversationsToMakeAvailable != null)
        {
            foreach(Conversation conversation in conversationsToMakeAvailable)
            {
                conversation.isAvailable = true;
            }
        }
    }
}

//Types of Interaction
public enum InteractableType
{
    CarryItem,
    CollectionItem,
    Conversation
}
