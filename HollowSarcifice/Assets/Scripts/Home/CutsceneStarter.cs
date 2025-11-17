using UnityEngine;

public class CutsceneStarter : MonoBehaviour
{
    public Animator anim;

    public void PlaySequence()
    {
        anim.SetTrigger("MoveNews");
    }

    public void restart()
    {
        anim.SetTrigger("Rebirth");
    }
    
}