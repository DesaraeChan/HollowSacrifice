using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    public static MoneyCounter Instance;
    public float money;
    public bool family = false;

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

    public void sendMoney()
    {
        if (checkMoney())
        {
            money -= setAmount();
            DayManager.Instance.NextDay();
        } else
        {
            Debug.Log("You poor");
        }
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
        if (family)
        {
            amount += 30;
        }
        return amount;
    }

    public void FamilyBool(bool val) { family = val; }
}