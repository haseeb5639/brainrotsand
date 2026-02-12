using UnityEngine;
using System.Collections.Generic;

public class BrainrotManager : MonoBehaviour
{
    public static BrainrotManager instance;

    [Header("References")]
    public Transform player;
    public Transform holdPoint;
    public MoneyAnimationController moneyAnimator;
    // NEW: Reference to the player's detection script
    public PlayerBaseDetector playerDetector;

    [Header("Settings")]
    public float interactDistance = 2.5f;

    public BrainrotInteraction activeBrainrot; // Brainrot currently highlighted for interaction
    private BrainrotInteraction heldBrainrot; // Brainrot currently being carried

    private List<string> randomNames = new List<string>
    { "Panda", "Monkey", "Cat", "Frog", "Bunny", "Duck", "Cow", "Tiger", "Dog", "Pig" };

    void Awake() => instance = this;

    void Start()
    {
        // Auto-assign names and rates to all brainrots
        var brainrots = FindObjectsOfType<BrainrotInteraction>();
        for (int i = 0; i < brainrots.Length; i++)
        {
            // Use a random name, but ensure we don't pick the same one twice in a simple way
            int nameIndex = Random.Range(0, randomNames.Count);
            string assignedName = randomNames[nameIndex];
            randomNames.RemoveAt(nameIndex); // Remove it so the next brainrot doesn't get the same name

            brainrots[i].brainrotName = assignedName;
            brainrots[i].pricePerSecond = (i + 1);
            brainrots[i].moneyAnimator = moneyAnimator;
            brainrots[i].UpdateInfoUI();

            // Refill names if we ran out
            if (randomNames.Count == 0)
            {
                randomNames = new List<string>
                { "Panda", "Monkey", "Cat", "Frog", "Bunny", "Duck", "Cow", "Tiger", "Dog", "Pig" };
            }
        }
    }

    public void OnBrainrotNear(BrainrotInteraction b)
    {
        activeBrainrot = b;
        b.ShowInfo(true);
    }

    public void OnBrainrotFar(BrainrotInteraction b)
    {
        if (activeBrainrot == b)
        {
            activeBrainrot = null;
            b.ShowInfo(false);
        }
    }

    // ------------------------------------------------------------

    public void Grab()
    {
        if (heldBrainrot != null) return;

        if (activeBrainrot != null)
        {
            heldBrainrot = activeBrainrot;
            heldBrainrot.PickUp(holdPoint);
            BrainrotUIManager.instance.ShowMessage($"Picked **{heldBrainrot.brainrotName}**");
            // Important: Clear activeBrainrot when grabbed so we don't try to grab it again
            activeBrainrot.ShowInfo(false);
            activeBrainrot = null;
        }
        else
            BrainrotUIManager.instance.ShowMessage("No brainrot nearby!");
    }

    public void Drop()
    {
        if (heldBrainrot == null)
        {
            BrainrotUIManager.instance.ShowMessage("Nothing to drop!");
            return;
        }

        // Pass the held item to the Player Detector to handle the drop logic
        bool success = playerDetector.TryDropHeldBrainrot(heldBrainrot);

        if (success)
        {
            // If the drop was successful (base was nearby and empty), clear the held item
            heldBrainrot = null;
        }
    }
    // In BrainrotManager.cs

    // ------------------------------------------------------------
    // NEW: Helper method for the PlayerBaseDetector
    public bool DropIsAvailable()
    {
        return heldBrainrot != null;
    }
}