// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Threading.Tasks;
// using System;

// public class UI_LoginAsGuest : MonoBehaviour
// {
//     [Header("Buttons")]
//     [SerializeField] private Button guestLoginButton;

//     [Header("Loading")]
//     [SerializeField] private GameObject loadingPanel;
//     [SerializeField] private TextMeshProUGUI loadingText;

//     [Header("Error")]
//     [SerializeField] private GameObject errorPanel;
//     [SerializeField] private TextMeshProUGUI errorText;
//     [SerializeField] private Button errorCloseButton;

//     private void Start()
//     {
//         FirebaseManager.OnError += ShowError;
//         FirebaseManager.OnAuthStateChanged += OnAuthSuccess;

//         if (guestLoginButton != null)
//         {
//             guestLoginButton.onClick.RemoveAllListeners();
//             guestLoginButton.onClick.AddListener(SignInAsGuest);
//         }

//         if (errorCloseButton != null)
//             errorCloseButton.onClick.AddListener(() => errorPanel.SetActive(false));

//         if (loadingPanel != null)
//             loadingPanel.SetActive(false);
//         if (errorPanel != null)
//             errorPanel.SetActive(false);
//     }

//     private void OnDestroy()
//     {
//         FirebaseManager.OnError -= ShowError;
//         FirebaseManager.OnAuthStateChanged -= OnAuthSuccess;
//     }

//     public async void SignInAsGuest()
//     {
//         guestLoginButton.interactable = false;
//         ShowLoading("Signing in as guest...");

//         bool success = await FirebaseManager.instance.SignInAnonymouslyAsync();

//         guestLoginButton.interactable = true;

//         if (!success)
//         {
//             HideLoading();
//         }
//     }

//     public async void OnAuthSuccess(string userId)
//     {
//         HideLoading();
//         try
//         {
//             int playerNumber = await MP_PlayerIDNumberManager.GetAndAssignPlayerNumber(userId);
//             string playerName = $"Player{playerNumber:D4}";
//             Debug.Log($"Tên người chơi: {playerName}");
//             // TODO: Hiển thị lên UI hoặc lưu vào đâu đó
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError($"Lỗi khi lấy player number: {ex.Message}");
//             ShowError("Không thể lấy số thứ tự người chơi. Vui lòng thử lại.");
//         }
//     }


//     private void ShowError(string message)
//     {
//         if (errorPanel != null && errorText != null)
//         {
//             errorText.text = message;
//             errorPanel.SetActive(true);
//         }
//     }

//     private void ShowLoading(string message = "Loading...")
//     {
//         if (loadingPanel != null && loadingText != null)
//         {
//             loadingText.text = message;
//             loadingPanel.SetActive(true);
//         }
//     }

//     private void HideLoading()
//     {
//         if (loadingPanel != null)
//             loadingPanel.SetActive(false);
//     }
// }