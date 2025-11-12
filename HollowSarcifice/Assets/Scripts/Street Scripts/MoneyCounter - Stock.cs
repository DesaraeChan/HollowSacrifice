using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    public static MoneyCounter Instance;
    public float money;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool checkMoney()
    {
        if (money - setAmount() < 0)
        {
            return false;
        }
        else
        {
            return true;
        }


    }

    public float setAmount()
    {
        float amount = 0;
        
        return amount;
    }
}