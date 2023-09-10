using DG.Tweening;
using UnityEngine;


public class SplashScreenController : MonoBehaviour
{

    [SerializeField] private RectTransform Title;
    [SerializeField] private RectTransform leftEye;
    [SerializeField] private RectTransform rightEye;
    private Sequence showTitleSequence;

    void OnEnable()
    {
        showTitleSequence = DOTween.Sequence();
        showTitleSequence
            .Append(Title.DOScale(1, 3))
            .SetEase(Ease.InOutBounce)
            .Append(leftEye.DORotate(new Vector3(0, 0, -90), 1))
            .Join(rightEye.DORotate(new Vector3(0, 0, 90), 1))
            .Append(leftEye.DORotate(new Vector3(0, 0, 90), 1))
            .Join(rightEye.DORotate(new Vector3(0, 0, -90), 1))
            .AppendCallback(() => CloseSplashScreen());

    }

    void CloseSplashScreen()
    {
        gameObject.SetActive(false);
    }
    
}