using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理类
/// </summary>
public class UIManager : MonoBehaviour
{
    public Button startBtn;
    public Button rankBtn;
    public Button quitBtn;
    public Button closeRankBtn;
    public Button usernameBtn;
    public InputField usernameInput;
    public GameObject startPanel;
    public GameObject usernamePanel;
    public GameObject rankPanel;

    void Awake()
    {
        // 注册按钮点击事件
        startBtn.onClick.AddListener(delegate { OnStartBtnClick(); });
        rankBtn.onClick.AddListener(delegate { OnRankBtnClick(); });
        quitBtn.onClick.AddListener(delegate { OnQuitBtnClick(); });
        closeRankBtn.onClick.AddListener(delegate { OnCloseRankBtnClick(); });
        usernameBtn.onClick.AddListener(delegate { OnUsernameBtnClick(); });
    }

    // 开始按钮
    void OnStartBtnClick()
    {
        usernameInput.text = "";
        usernamePanel.SetActive(true);
    }

    // 排行榜按钮
    void OnRankBtnClick()
    {
        rankPanel.SetActive(true);
        // 从服务端获取排行榜数据
        Server.Instance.StartCoroutine("DownloadScore");
    }

    // 退出按钮
    void OnQuitBtnClick()
    {
        Application.Quit();
    }

    // 关闭排行榜按钮
    void OnCloseRankBtnClick()
    {
        rankPanel.SetActive(false);
    }

    // 确认用户名按钮
    void OnUsernameBtnClick()
    {
        usernamePanel.SetActive(false);
        startPanel.SetActive(true);
        GameManager.Instance.username = usernameInput.text;
    }
}
