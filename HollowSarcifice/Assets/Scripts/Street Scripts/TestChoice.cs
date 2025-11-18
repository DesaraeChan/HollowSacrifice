using UnityEngine;

public class TestSetChoice : MonoBehaviour
{
    void Start()
    {
        // Pretend the player picked choice A (0) or B (1)
        DecisionTracker.Instance.SetChoice("Homeless", 0);   // TRY 0 or 1
    }
}