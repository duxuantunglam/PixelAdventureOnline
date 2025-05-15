using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerNamePanel;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Button submitButton;

    public void SubmitName()
    {
        FusionManager.instance.ConnectToLobby(playerNameInputField.text);
        playerNamePanel.SetActive(false);
    }

    public void ActivateButton()
    {
        submitButton.interactable = true;
    }
}
