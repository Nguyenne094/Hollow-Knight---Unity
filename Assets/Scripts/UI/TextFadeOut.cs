using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TextFadeOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private string triggerTag = "FadeOutTrigger";
    [SerializeField] private float fadeDuration = 1f;
    
    private bool hasFadedOut = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        if (!hasFadedOut && other.CompareTag(triggerTag))
        {
            FadeOutText();
            hasFadedOut = true;
        }
    }

    private void FadeOutText()
    {
        Debug.Log("Fading out text");
        canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => {
            canvasGroup.gameObject.SetActive(false);
        });
    }
}