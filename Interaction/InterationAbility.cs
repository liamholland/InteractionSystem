using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterationAbility : MonoBehaviour
{
    public float interactionRange;
    public LayerMask whatIsInteractable;
    public Text interactionText;
    public Animator animator; //Animator for Interaction Text
    public NotificationManager notificationManager; //Delete if theres no NotificationManagerScript

    public static bool canInteract = true; //Be careful with this as when false it will not update if there is
                                           //no Interactable available, so if changed to false while there are
                                           //instructions on screen, those instructions will remain there until
                                           //this is back to true

    private float distanceToObject;
    private float currentShortestDistance;
    private GameObject closestObject;
    private InteractableObject currentObject;

    void Update()
    {
        if (canInteract)
        {
            SetClosestInteractable();
            if (Input.GetButtonDown("Interact"))
            {
                InteractWith(closestObject);
                notificationManager.DisplayNotification(currentObject.notificationToGive);
            }
        }
    }

    //Calls the Interaction Function on the closestObject when the player interacts with it
    private void InteractWith(GameObject closestObject)
    {
        closestObject.GetComponent<InteractableObject>().DoInteraction();
    }

    //This finds all the interactable objects in the area. If there are more than 1 it sorts through
    //them to find the closest one
    private void SetClosestInteractable()
    {
        currentShortestDistance = 0;
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, interactionRange, whatIsInteractable);
        if(interactables.Length != 0)
        {
            if(interactables.Length > 1)
            {
                foreach (Collider2D interactable in interactables)
                {
                    distanceToObject = Vector2.Distance(transform.position, interactable.gameObject.transform.position);
                    if (currentShortestDistance == 0 || distanceToObject < currentShortestDistance)
                    {
                        closestObject = interactable.gameObject;
                        currentObject = closestObject.GetComponent<InteractableObject>();
                        if (currentObject.hideInstructions)
                        {
                            interactionText.text = currentObject.interactionText;
                            SetShowToTrue();
                        }
                        else
                        {
                            interactionText.text = "Press E to " + currentObject.interactionText;
                            SetShowToTrue();
                        }
                        currentShortestDistance = distanceToObject;
                    }
                }
            }
            else
            {
                closestObject = interactables[0].gameObject;
                currentObject = closestObject.GetComponent<InteractableObject>();
                if (currentObject.hideInstructions)
                {
                    interactionText.text = currentObject.interactionText;
                    SetShowToTrue();
                }
                else
                {
                    interactionText.text = "Press E to " + currentObject.interactionText;
                    SetShowToTrue();
                }
                currentShortestDistance = distanceToObject;
            }
        }
        else
        {
            animator.SetBool("show", false);
        }
    }

    //Helps animate the text
    private void SetShowToTrue()
    {
        if (animator.GetBool("show") != true)
        {
            animator.SetBool("show", true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
