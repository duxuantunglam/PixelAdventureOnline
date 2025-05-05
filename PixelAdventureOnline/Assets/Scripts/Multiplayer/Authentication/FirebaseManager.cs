using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel, notificationPanel;

    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupConfirmPassword, signupUserName, forgetPassEmail;

    public TMP_Text noti_Title_Text, noti_Message_Text, profileUserName_Text;

    public Toggle rememberMe;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    bool isSignIn = false;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        // Do Login
        SignInUser(loginEmail.text, loginPassword.text);
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupConfirmPassword.text) || string.IsNullOrEmpty(signupUserName.text))
        {
            showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        // Do SignUp
        CreateUser(signupEmail.text, signupPassword.text, signupUserName.text);
    }

    public void forgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        // Do Forget Password
    }

    private void showNotificationMessage(string title, string message)
    {
        noti_Title_Text.text = "" + title;
        noti_Message_Text.text = "" + message;

        notificationPanel.SetActive(true);
    }

    public void CloseNoti_Panel()
    {
        noti_Title_Text.text = "";
        noti_Message_Text.text = "";

        notificationPanel.SetActive(false);
    }

    public void LogOut()
    {
        auth.SignOut();
        profileUserName_Text.text = "";
        OpenLoginPanel();
    }

    void CreateUser(string email, string password, string userName)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            UpdateUserProfile(userName);
        });
    }

    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            profileUserName_Text.text = "" + result.User.DisplayName;
            OpenProfilePanel();
        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void UpdateUserProfile(string userName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = userName,
                PhotoUrl = new System.Uri("https://placehold.co/600x400"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

                showNotificationMessage("Alert", "Account Successfully Created!");
            });
        }
    }

    bool isSigned = false;
    void Update()
    {
        if (isSignIn)
        {
            if (!isSigned)
            {
                isSignIn = true;
                profileUserName_Text.text = "" + user.DisplayName;
                OpenProfilePanel();
            }
        }
    }
    // public static FirebaseManager instance;

    // public static event Action<string> OnAuthStateChanged;
    // public static event Action<string> OnError;

    // private FirebaseAuth auth;
    // private FirebaseUser user;
    // public DatabaseReference DBreference { get; private set; }

    // private FirebaseApp app;

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }

    //     InitializeFirebase();
    // }

    // public static bool isFirebaseReady = false;
    // private void InitializeFirebase()
    // {
    //     Debug.Log("Setting up Firebase Auth");

    //     FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    //     {
    //         var dependencyStatus = task.Result;
    //         if (dependencyStatus == DependencyStatus.Available)
    //         {
    //             var options = FirebaseApp.DefaultInstance.Options;
    //             options.DatabaseUrl = new Uri("https://pixeladventureonline-default-rtdb.asia-southeast1.firebasedatabase.app");

    //             app = FirebaseApp.Create(options, "PixelAdventure");

    //             auth = FirebaseAuth.GetAuth(app);

    //             auth.StateChanged += AuthStateChanged;
    //             AuthStateChanged(this, null);

    //             isFirebaseReady = true;
    //             Debug.Log("Firebase Auth initialized with custom app!");
    //         }
    //         else
    //         {
    //             Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
    //             OnError?.Invoke("Failed to initialize Firebase");
    //         }
    //     });
    // }

    // private void AuthStateChanged(object sender, EventArgs eventArgs)
    // {
    //     if (auth.CurrentUser != user)
    //     {
    //         bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
    //         user = auth.CurrentUser;

    //         if (signedIn)
    //         {
    //             Debug.Log($"Signed in: {user.UserId}");
    //             OnAuthStateChanged?.Invoke(user.UserId);

    //             DBreference = FirebaseDatabase.GetInstance(app).RootReference;
    //         }
    //     }
    // }

    // private void OnDestroy()
    // {
    //     if (auth != null)
    //     {
    //         auth.StateChanged -= AuthStateChanged;
    //     }
    // }

    // #region Anonymous Authentication
    // public async Task<bool> SignInAnonymouslyAsync()
    // {
    //     try
    //     {
    //         var result = await auth.SignInAnonymouslyAsync();
    //         user = result.User;
    //         Debug.Log($"Anonymous user created: {user.UserId}");
    //         return true;
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.LogError($"Failed to sign in anonymously: {ex.Message}");
    //         OnError?.Invoke("Failed to sign in as guest");
    //         return false;
    //     }
    // }
    // #endregion

    // #region Auth State
    // public bool IsSignedIn()
    // {
    //     return user != null;
    // }

    // public string GetUserId()
    // {
    //     return user?.UserId ?? string.Empty;
    // }

    // public bool IsAnonymous()
    // {
    //     return user?.IsAnonymous ?? false;
    // }

    // public void SignOut()
    // {
    //     if (auth != null)
    //     {
    //         auth.SignOut();
    //         user = null;
    //     }
    // }
    // #endregion
}