using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private RectTransform LoadingBar;
    [SerializeField] private RectTransform ProgressBar;
    [SerializeField] private TextMeshProUGUI ProgressText;
    [SerializeField] private TextMeshProUGUI LoadingText;
    // Start is called before the first frame update
    void Start()
    {
        float loadingBarWidth = LoadingBar.sizeDelta.x;
        ProgressBar.DOSizeDelta(new Vector2(1375f, ProgressBar.sizeDelta.y), 2f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            SceneManager.LoadScene("VoxelStrikeHome");
        });
        float progress = 0f;
        DOTween.To(() => progress, progressText => ProgressText.text = ((int)progressText).ToString() + "%", 100f, 2f).SetEase(Ease.InCubic);

        string loading = "LOADING";
        DOTween.To(() => loading, loadingText => LoadingText.text = loadingText, "LOADING...", 0.4f).SetLoops(-1, LoopType.Restart);

    }

    private void OnDestroy()
    {

    }
}
