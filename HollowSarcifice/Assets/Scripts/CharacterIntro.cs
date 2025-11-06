using UnityEngine;

public class CharacterIntro : MonoBehaviour
{
    [SerializeField] private GameObject textBox; // assign your UI text box in Inspector

    // This is called automatically by the animation event
    public void ShowTextBox()
    {
        textBox.SetActive(true);
    }
}
