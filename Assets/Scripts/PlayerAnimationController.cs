using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator anim;
    public GameObject hammer;

    private bool hammerOn = false;
    private bool isFallen = false;

    // Called by Attack Button
    public void OnAttackButton()
    {
        if (isFallen) return;

        hammerOn = !hammerOn;
        hammer.SetActive(hammerOn);

        anim.SetTrigger("Attack");
    }

    // Called when AI hits player
    public void PlayFall(float delay = 2f)
    {
        if (isFallen) return;

        isFallen = true;

        // hide hammer when falling
        hammer.SetActive(false);
        hammerOn = false;

        anim.SetTrigger("Fall");

        Invoke(nameof(PlayGetUp), delay);
    }

    private void PlayGetUp()
    {
        anim.SetTrigger("GetUp");
        isFallen = false;
    }
}
