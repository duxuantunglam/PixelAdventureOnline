using PixelAdventureOnline.FusionBites;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] public GameObject playerNamePanel;
    [SerializeField] public TMP_InputField playerNameInputField;
    [SerializeField] public Button submitButton;

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
