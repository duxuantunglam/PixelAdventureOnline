// using Firebase.Database;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;

// public class MP_PlayerIDNumberManager : MonoBehaviour
// {
//     public static async Task<int> GetAndAssignPlayerNumber(string userId)
//     {
//         int assignedNumber = 0;
//         DatabaseReference dbRef = FirebaseManager.instance.DBreference;

//         var transactionResult = await dbRef.RunTransaction(mutableData =>
//         {
//             var data = mutableData.Value as Dictionary<string, object> ?? new Dictionary<string, object>();
//             Debug.Log($"[Transaction] Current data keys: {string.Join(", ", data.Keys)}");

//             int lastNumber = 0;
//             if (data.ContainsKey("lastPlayerNumber"))
//             {
//                 if (int.TryParse(data["lastPlayerNumber"].ToString(), out int parsedNumber))
//                     lastNumber = parsedNumber;
//                 else
//                     Debug.LogWarning("[Transaction] lastPlayerNumber parse failed, defaulting to 0");
//             }

//             lastNumber++;
//             data["lastPlayerNumber"] = lastNumber;

//             // Gán cho user
//             if (!data.ContainsKey("users"))
//                 data["users"] = new Dictionary<string, object>();

//             var users = data["users"] as Dictionary<string, object>;
//             if (!users.ContainsKey(userId))
//                 users[userId] = new Dictionary<string, object>();

//             var userData = users[userId] as Dictionary<string, object>;
//             userData["playerNumber"] = lastNumber;
//             users[userId] = userData;
//             data["users"] = users;

//             mutableData.Value = data;
//             assignedNumber = lastNumber;

//             Debug.Log($"[Transaction] Assigned playerNumber={lastNumber} to userId={userId}");

//             return TransactionResult.Success(mutableData);
//         });

//         return assignedNumber;
//     }
// }