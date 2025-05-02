using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    public static event Action<string> OnAuthStateChanged;
    public static event Action<string> OnError;

    private FirebaseAuth auth;
    private FirebaseUser user;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;

                Debug.Log("Firebase Auth is ready!");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                OnError?.Invoke("Failed to initialize Firebase");
            }
        });
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"Signed in: {user.UserId}");
                OnAuthStateChanged?.Invoke(user.UserId);
            }
        }
    }

    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
        }
    }

    #region Anonymous Authentication
    public async Task<bool> SignInAnonymouslyAsync()
    {
        try
        {
            var result = await auth.SignInAnonymouslyAsync();
            user = result.User;
            Debug.Log($"Anonymous user created: {user.UserId}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to sign in anonymously: {ex.Message}");
            OnError?.Invoke("Failed to sign in as guest");
            return false;
        }
    }
    #endregion

    #region Auth State
    public bool IsSignedIn()
    {
        return user != null;
    }

    public string GetUserId()
    {
        return user?.UserId ?? string.Empty;
    }

    public bool IsAnonymous()
    {
        return user?.IsAnonymous ?? false;
    }

    public void SignOut()
    {
        if (auth != null)
        {
            auth.SignOut();
            user = null;
        }
    }
    #endregion
}