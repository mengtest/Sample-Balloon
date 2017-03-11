using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data
{
    public int id;
    public string username;
    public int score;
}

public class Server : MonoBehaviour
{
    // 单例模式，保证全局只存在一个实例
    private static Server _instance;
    public static Server Instance { get { return _instance; } }

    // RankItemUI列表
    public RankItemUI[] rankItemUI;

    // 服务端上传地址
    private const string uploadURL = "http://www.littleredhat1997.com/games/balloon_upload.php";
    // 服务端下载地址
    private const string downloadURL = "http://www.littleredhat1997.com/games/balloon_download.php";

    void Awake()
    {
        // 创建单例
        _instance = this;
    }

    // 从服务端下载分数
    public IEnumerator UploadScore(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", GameManager.Instance.username);
        form.AddField("score", score.ToString());

        WWW www = new WWW(uploadURL, form);

        // 等待www响应
        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
    }

    // 向服务端上传分数
    public IEnumerator DownloadScore()
    {
        WWW www = new WWW(downloadURL);

        // 等待www响应
        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            // 反序列化
            var dict = MiniJSON.Json.Deserialize(www.text) as Dictionary<string, object>;

            // 数组下标
            int index = 0;

            // 防止排行榜为空报错
            if (dict != null)
            {
                foreach (object v in dict.Values)
                {
                    // 构造Data
                    Data data = new Data();
                    MiniJSON.Json.ToObject(data, v);

                    // 显示UI
                    rankItemUI[index].gameObject.SetActive(true);
                    rankItemUI[index].UpdateUI(data.username, data.score);
                    ++index;
                }
            }

            // 如果排行榜数量不足，多余的需要隐藏
            for (int i = index; i < rankItemUI.Length; ++i)
            {
                rankItemUI[i].gameObject.SetActive(false);
            }
        }
    }
}
