using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
     public Animator anim;
    PlayerManager playerManager;
    public void PlayTargetAnimation(string targetAnim, bool IsInteracting)
    {
        anim.applyRootMotion = IsInteracting;
        anim.SetBool("IsInteracting", IsInteracting);

        if (playerManager != null)
        {
            playerManager.IsInteracting = IsInteracting;
        }
        anim.CrossFade(targetAnim, 0.2f);
    }
}
