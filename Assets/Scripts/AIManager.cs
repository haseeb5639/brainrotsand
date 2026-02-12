using UnityEngine;
using System.Collections.Generic;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;

    public List<AISimpleMover> allAIs;   // Drag AI1, AI2, AI3
    public Transform player;             // Drag Player Transform

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerEnteredBase(BaseOwner owner)
    {
        foreach (var ai in allAIs)
        {
            if (ai.owner == owner)
                ai.StartDefense(player);
        }
    }

    public void PlayerExitedBase(BaseOwner owner)
    {
        foreach (var ai in allAIs)
        {
            if (ai.owner == owner)
                ai.StopDefense();
        }
    }
}
