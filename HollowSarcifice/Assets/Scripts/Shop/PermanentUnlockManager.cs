using UnityEngine;

public class PermanentUnlockManager : MonoBehaviour
{
    public static PermanentUnlockManager Instance { get; private set; }

    [Header("Object permanently visible after unlock")]
    [SerializeField] private GameObject homelessPermanentObject;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // stays across scenes
    }

    void Start()
    {
        RefreshUnlockState();
            homelessPermanentObject.SetActive(false);
    }

    public void ActivateHomelessObject()
    {
        if (homelessPermanentObject != null)
            homelessPermanentObject.SetActive(true);

        // Save permanently
        DecisionTracker.Instance.SetChoice("Homeless_Unlock", 1);
    }

    public void RefreshUnlockState()
    {
        // When scenes load, check stored state
        if (DecisionTracker.Instance.TryGetChoice("Homeless_Unlock", out int unlock) && unlock == 1)
        {
            if (homelessPermanentObject != null)
                homelessPermanentObject.SetActive(true);
        }
    }
}
