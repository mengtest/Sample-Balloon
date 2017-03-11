using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
    WAIT,  // 游戏等待
    READY,  // 游戏准备
    START,  // 游戏开始
    END,  // 游戏结束
}

/// <summary>
/// 游戏管理类
/// </summary>
public class GameManager : MonoBehaviour
{
    // 单例模式，保证全局只存在一个实例
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    // 游戏状态
    public GameState gameState;
    // 游戏音效
    private AudioSource audioSource;
    private AudioClip bangClip;
    // 游戏视图
    public CountDown countDown;
    public Text scoreText;
    public Text timerText;
    public Text resultText;
    public Image successImage;
    public Button comfirmBtn;
    public Button slowBtn;
    public Button normalBtn;
    public Button fastBtn;
    private Button[] balloonBtn;
    private Image[] balloonImage;
    private Sprite[] balloonSprite = new Sprite[3];
    // 游戏面板
    public GameObject startPanel;
    public GameObject upContainer;
    public GameObject balloonContainer;
    public GameObject btnContainer;
    public GameObject gameOver;

    private float timer;  // 计时器
    private const float TIME = 60.0f;  // 游戏时间
    private const int COUNT_POS = 16;  // 气球所有数量
    private const int COUNT_COR = 3;  // 气球颜色种类

    private bool isOK;  // 是否允许生成气球
    private float rate;  // 生成气球速度
    private const float slow = 0.4f;  // 减速
    private const float normal = 0.2f;  // 正常
    private const float fast = 0.1f;  // 加速

    public string username { get; set; }
    private int score;

    void Awake()
    {
        // 创建单例
        _instance = this;
        // 获取音效组件
        audioSource = GetComponent<AudioSource>();
        // 获取音效资源
        bangClip = Resources.Load("bang") as AudioClip;

        // 注册按钮事件
        comfirmBtn.onClick.AddListener(delegate { OnComfirmBtnClick(); });
        slowBtn.onClick.AddListener(delegate { OnSlowBtnClick(); });
        normalBtn.onClick.AddListener(delegate { OnNormalBtnClick(); });
        fastBtn.onClick.AddListener(delegate { OnFastBtnClick(); });

        // 获取所有气球Button按钮
        balloonBtn = balloonContainer.GetComponentsInChildren<Button>();
        // 获取所有气球Image组件
        balloonImage = balloonContainer.GetComponentsInChildren<Image>();
        // 获取气球精灵
        balloonSprite[0] = Resources.Load("balloon_blue", new Sprite().GetType()) as Sprite;
        balloonSprite[1] = Resources.Load("balloon_green", new Sprite().GetType()) as Sprite;
        balloonSprite[2] = Resources.Load("balloon_yellow", new Sprite().GetType()) as Sprite;

        // 注册所有气球Button组件
        foreach (Button btn in balloonBtn)
        {
            btn.onClick.AddListener(delegate { OnBalloonClick(int.Parse(btn.name)); });
        }
    }

    void OnEnable()
    {
        // 每局开始前的初始化操作
        GameReady();
    }

    void Update()
    {
        if (gameState == GameState.END)
        {
            return;
        }
        // 游戏逻辑
        switch (gameState)
        {
            case GameState.WAIT:
                // 游戏等待，不要删除
                break;
            case GameState.READY:
                upContainer.SetActive(true);
                balloonContainer.SetActive(true);
                btnContainer.SetActive(true);

                timerText.text = TIME.ToString();
                scoreText.text = 0.ToString();

                gameState = GameState.START;
                break;
            case GameState.START:
                // 计时器减去每帧时间
                timer -= Time.deltaTime;
                // 向上取整
                timerText.text = Mathf.Ceil(timer).ToString();
                // 判断是否时间结束
                if (timer <= 0)
                {
                    GameOver();
                }
                // 判断是否生成气球
                if (isOK)
                {
                    StartCoroutine("SpawnBalloon");
                }
                // 判断是否游戏结束
                if (Check())
                {
                    GameOver();
                }
                break;
        }
    }

    // 游戏准备
    void GameReady()
    {
        timer = TIME;
        isOK = true;
        rate = normal;

        upContainer.SetActive(false);
        balloonContainer.SetActive(false);
        btnContainer.SetActive(false);
        gameOver.SetActive(false);
        countDown.gameObject.SetActive(true);

        // 取消所有气球Image组件
        foreach (Image image in balloonImage)
        {
            image.enabled = false;
        }
    }

    // 游戏结束
    void GameOver()
    {
        gameState = GameState.END;

        upContainer.SetActive(false);
        balloonContainer.SetActive(false);
        btnContainer.SetActive(false);
        gameOver.SetActive(true);

        // 战斗结算
        resultText.text = "@" + scoreText.text + "@";

        // 上传分数到服务端
        Server.Instance.StartCoroutine("UploadScore", score);
    }

    IEnumerator SpawnBalloon()
    {
        successImage.enabled = false;
        isOK = false;

        // 暂停时间
        yield return new WaitForSeconds(rate);

        // 随机位置
        int randomPos = Random.Range(0, COUNT_POS);
        balloonImage[randomPos].enabled = true;
        // 随机颜色
        int randomCor = Random.Range(0, COUNT_COR);
        balloonImage[randomPos].sprite = balloonSprite[randomCor];

        isOK = true;
    }

    void OnBalloonClick(int index)
    {
        score++;
        scoreText.text = score.ToString();
        balloonImage[index].enabled = false;

        // 显示成功
        successImage.enabled = true;
        // 播放音效
        audioSource.PlayOneShot(bangClip);
    }

    // 确认按钮
    void OnComfirmBtnClick()
    {
        startPanel.SetActive(false);
    }

    // 减速按钮
    void OnSlowBtnClick()
    {
        rate = slow;
    }
    // 正常按钮
    void OnNormalBtnClick()
    {
        rate = normal;
    }
    // 加速按钮
    void OnFastBtnClick()
    {
        rate = fast;
    }

    // 游戏失败返回True
    bool Check()
    {
        // 判断是否存在气球
        foreach (Image balloon in balloonImage)
        {
            // 存在气球不可用表示游戏继续
            if (!balloon.enabled)
            {
                return false;
            }
        }
        // 所有气球可用表示游戏结束
        return true;
    }
}
