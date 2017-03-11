using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RankItem视图
/// </summary>
public class RankItemUI : MonoBehaviour
{
    public Text usernameText;
    public Text scoreText;

    public void UpdateUI(string username, int score)
    {
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}
