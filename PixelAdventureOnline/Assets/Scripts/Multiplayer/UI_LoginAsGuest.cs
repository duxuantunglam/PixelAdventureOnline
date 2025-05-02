using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class UI_LoginAsGuest : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button guestLoginButton;

    [Header("Loading")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Error")]
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Button errorCloseButton;

    private void Start()
    {
        FirebaseManager.OnError += ShowError;
        FirebaseManager.OnAuthStateChanged += OnAuthSuccess;

        if (guestLoginButton != null)
            guestLoginButton.onClick.AddListener(SignInAsGuest);

        if (errorCloseButton != null)
            errorCloseButton.onClick.AddListener(() => errorPanel.SetActive(false));

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
        if (errorPanel != null)
            errorPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        FirebaseManager.OnError -= ShowError;
        FirebaseManager.OnAuthStateChanged -= OnAuthSuccess;
    }

    public async void SignInAsGuest()
    {
        guestLoginButton.interactable = false;
        ShowLoading("Signing in as guest...");

        bool success = await FirebaseManager.instance.SignInAnonymouslyAsync();

        guestLoginButton.interactable = true;

        if (!success)
        {
            HideLoading();
        }
    }


    private void OnAuthSuccess(string userId)
    {
        HideLoading();
        // TODO: Chuyển scene hoặc hiển thị menu chính
        Debug.Log($"Successfully signed in with ID: {userId}");
    }

    private void ShowError(string message)
    {
        if (errorPanel != null && errorText != null)
        {
            errorText.text = message;
            errorPanel.SetActive(true);
        }
    }

    private void ShowLoading(string message = "Loading...")
    {
        if (loadingPanel != null && loadingText != null)
        {
            loadingText.text = message;
            loadingPanel.SetActive(true);
        }
    }

    private void HideLoading()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}