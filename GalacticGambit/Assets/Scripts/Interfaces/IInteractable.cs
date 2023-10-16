using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //The function used to display information when an object becomes interactable.
    public void onInteractable(bool state);

    //The function used to trigger the interaction when an object is interacted with.
    public void onInteract();

    public bool isInteractable();

    public string interactionKey();

    public void repair(int experience, int modifier);

    public void man();

    public void toggleEnabled();

    public void back();

    public bool amIEnabled();
}
