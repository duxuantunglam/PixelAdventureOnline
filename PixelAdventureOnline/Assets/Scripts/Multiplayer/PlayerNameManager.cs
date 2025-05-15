using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInputField;

    public void SubmitName()
    {
        FusionManager.instance._playerName = playerNameInputField.text;
    }
}
