using UnityEngine;

public class CutsceneStarter : MonoBehaviour
{
    public Animator anim;

    public void PlaySequence()
    {
        anim.SetTrigger("MoveNews");
        SoundManager.Instance.PlaySFX("Letter");
    }

    public void restart()
    {
        anim.SetTrigger("Rebirth");
    }
    
}