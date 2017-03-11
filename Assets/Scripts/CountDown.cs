using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 倒计时
/// </summary>
public class CountDown : MonoBehaviour
{
    public Image numberImage;
    public Transform fireTransform;

    private Sprite numberOne;
    private Sprite numberTwo;
    private Sprite numberThree;
    private Sprite numberGo;
    private bool isRotate = true;  // 是否旋转
    private const float oneSecond = 1.0f;  // 一秒的时间
    private const float rotateSpeed = 360.0f;  // 旋转速度

    void Awake()
    {
        // 获取资源
        numberOne = Resources.Load("countdown1", new Sprite().GetType()) as Sprite;
        numberTwo = Resources.Load("countdown2", new Sprite().GetType()) as Sprite;
        numberThree = Resources.Load("countdown3", new Sprite().GetType()) as Sprite;
        numberGo = Resources.Load("countdown_go", new Sprite().GetType()) as Sprite;
    }

    void Update()
    {
        // 判断是否旋转
        if (isRotate)
        {
            // 火花逆时针旋转
            fireTransform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    // 当自身可用时自动被调用
    void OnEnable()
    {
        // 开启协程
        StartCoroutine("Run");
    }

    // 倒计时协程
    IEnumerator Run()
    {
        // 开始旋转
        isRotate = true;

        numberImage.sprite = numberThree;

        // 暂停一秒
        yield return new WaitForSeconds(oneSecond);

        numberImage.sprite = numberTwo;

        // 暂停一秒
        yield return new WaitForSeconds(oneSecond);

        numberImage.sprite = numberOne;

        // 暂停一秒
        yield return new WaitForSeconds(oneSecond);

        numberImage.sprite = numberGo;

        // 停止旋转
        isRotate = false;

        // 视觉残留
        yield return new WaitForSeconds(0.2f);

        // 隐藏自身
        this.gameObject.SetActive(false);

        // 通知游戏准备开始
        GameManager.Instance.gameState = GameState.READY;
    }
}
