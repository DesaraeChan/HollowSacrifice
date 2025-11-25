using UnityEngine;
using TMPro;
public class RepPopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Animator animator;

    void OnEnable()
    {
        Debug.Log("[RepPopupUI] Enabled, subscribing to OnReputationApplied");
        CharacterManager.OnReputationApplied += HandleRep;
    }

    // void OnDisable()
    // {
    //     Debug.Log("[RepPopupUI] Disabled, unsubscribing from OnReputationApplied");
    //     CharacterManager.OnReputationApplied -= HandleRep;
    // }

    private void HandleRep(int delta)
    {
        Debug.Log($"[RepPopupUI] HandleRep called with delta = {delta}");

        if (delta == 0) return;

        if (amountText){
            if (delta > 0){
                amountText.text = $"+{delta} Rep Point";
                SoundManager.Instance.PlaySFX("RepUp");
            }else{
                amountText.text = $"{delta} Rep Point";
                SoundManager.Instance.PlaySFX("RepDown");
            }
                
        }

        if (animator) 
        {
             string stateName = delta> 0 ? "Up" : "Down";

        // Force the animation to restart even if already in that state
        animator.Play(stateName, 0, 0f);
        }
       
        //animator.SetTrigger(delta > 0 ? "Up" : "Down");

        
    }
}
