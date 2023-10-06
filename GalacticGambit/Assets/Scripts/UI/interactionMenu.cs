using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class interactionMenu : MonoBehaviour
{
    [SerializeField] List<Image> slotList;
    [SerializeField] List<string> actionList;
    [SerializeField] TMP_Text interactionText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(highlightOnClick(slotList[0]));
            doAction(actionList[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(highlightOnClick(slotList[1]));
            doAction(actionList[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(highlightOnClick(slotList[2]));
            doAction(actionList[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(highlightOnClick(slotList[3]));
            doAction(actionList[3]);
        }
    }
    public void updateInteractionText(string value)
    {
        interactionText.text = value;
    }

    public void updateInteractionOptions(List<Interaction> interactions)
    {
        for(int i = 0; i < interactions.Count; i++)
        {
            slotList[3-i].sprite = interactions[3-i].icon;
            actionList.Add(interactions[i].action);
        }
        
    }

    public IEnumerator highlightOnClick(Image icon)
    {
        icon.gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        icon.gameObject.SetActive(true);

    }

    void doAction(string action)
    {
        if(action == "Repair")
        {
            gamemanager.instance.topDownPlayerController.selectedCrewMember.selectedInteraction.repair(gamemanager.instance.topDownPlayerController.selectedCrewMember.repairExperience, gamemanager.instance.topDownPlayerController.selectedCrewMember.repairModifier);
        }
        else if (action == "Man")
        {
            gamemanager.instance.topDownPlayerController.selectedCrewMember.selectedInteraction.man();
        }
        else if (action == "toggleEnabled")
        {
            gamemanager.instance.topDownPlayerController.selectedCrewMember.selectedInteraction.toggleEnabled();
        }
        else if (action == "Back")
        {
            gamemanager.instance.topDownPlayerController.selectedCrewMember.selectedInteraction.back();
        }
    }

    public void clearList()
    {
        actionList.Clear();
    }
}
