using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MP_PlayerIDNumberManager : MonoBehaviour
{
    public static async Task<int> GetAndAssignPlayerNumber(string userId)
    {
        int playerNumber = 0;
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        await dbRef.RunTransaction(mutableData =>
        {
            var data = mutableData.Value as Dictionary<string, object> ?? new Dictionary<string, object>();

            int lastNumber = 0;
            if (data.ContainsKey("lastPlayerNumber"))
                lastNumber = int.Parse(data["lastPlayerNumber"].ToString());

            lastNumber++;
            data["lastPlayerNumber"] = lastNumber;

            // Gán cho user
            if (!data.ContainsKey("users"))
                data["users"] = new Dictionary<string, object>();

            var users = data["users"] as Dictionary<string, object>;
            if (!users.ContainsKey(userId))
                users[userId] = new Dictionary<string, object>();

            var userData = users[userId] as Dictionary<string, object>;
            userData["playerNumber"] = lastNumber;
            users[userId] = userData;
            data["users"] = users;

            mutableData.Value = data;
            playerNumber = lastNumber;
            return TransactionResult.Success(mutableData);
        });

        return playerNumber;
    }
}