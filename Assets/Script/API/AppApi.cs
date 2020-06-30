using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

public class AppApi
{
    private static readonly string BaseUrl = "http://localhost/apiTest/";
    public static async UniTask<UserData> GetUserData(string userHash)
    {
        var url = BaseUrl + "GetUserDataAPI.php";
        var form = new WWWForm();
        form.AddField("user_hash", userHash);
        var result = UnityWebRequest.Post(url, form);

        await result.SendWebRequest().ToUniTask();

        if (result.isNetworkError)
        {
            Debug.LogError("はーや、おわりー");
        }

        return JsonUtility.FromJson<UserData>(result.downloadHandler.text);
    }

    public static async UniTask<string> CreateUserData(UserData userData)
    {
        var url = BaseUrl + "CreateUserDataAPI.php";
        var form = new WWWForm();
        form.AddField("name", userData.Name);
        var result = UnityWebRequest.Post(url, form);

        await result.SendWebRequest().ToUniTask();

        if (result.isNetworkError)
        {
            Debug.LogError("はーや、おわりー");
        }

        Debug.Log(result.downloadHandler.text);

        return result.downloadHandler.text;
    }

    public static async UniTask<bool> SaveUserData(UserData userData)
    {
        var url = BaseUrl + "SaveUserDataAPI.php";
        var form = new WWWForm();
        var json = JsonUtility.ToJson(userData);
        var userHash = PlayerPrefs.GetString(UserData.USER_HASH_KEY);
        form.AddField("user_data", json);
        form.AddField("user_hash", userHash);

        var result = UnityWebRequest.Post(url, form);

        await result.SendWebRequest().ToUniTask();

        if (result.isNetworkError)
        {
            Debug.LogError("ユーザーデータの保存に失敗");
        }

        Debug.Log(result.downloadHandler.text);

        return result.isNetworkError;
    }

    public static async UniTask<List<UserData>> GetRankUserData()
    {
        var url = BaseUrl + "GetUserRanking.php";
        var form = new WWWForm();
        var result = UnityWebRequest.Post(url, form);

        await result.SendWebRequest().ToUniTask();

        if (result.isNetworkError)
        {
            Debug.LogError("ユーザーデータの保存に失敗");
        }

        Debug.Log("UserData:" +result.downloadHandler.text);
        var list = JsonHelper.FromJson<UserData>(result.downloadHandler.text);
        return list as List<UserData>;
    }
}
