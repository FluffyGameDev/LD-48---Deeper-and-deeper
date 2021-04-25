using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UITutorialSlide
{
    public Sprite TutorialImage;
    public string TutorialText;
}

public class UITutorialPanelController : MonoBehaviour
{
    [SerializeField]
    private Image TutorialImage;
    [SerializeField]
    private Text TutorialText;
    [SerializeField]
    private Button NextButton;

    [SerializeField]
    private PlayerChannel PlayerChannel;

    [SerializeField]
    private UITutorialSlide[] Slides;

    private uint m_CurrentSlide = 0;

    private void Start()
    {
        PlayerChannel.RaiseMovementEnabled(false);

        NextButton.onClick.AddListener(RequestNextSlide);

        ShowSlide(0);
    }

    private void OnDestroy()
    {
        NextButton.onClick.RemoveListener(RequestNextSlide);
    }

    private void RequestNextSlide()
    {
        ShowSlide(m_CurrentSlide + 1);
    }

    private void ShowSlide(uint index)
    {
        m_CurrentSlide = index;
        if (m_CurrentSlide < Slides.Length)
        {
            UITutorialSlide currentSlide = Slides[m_CurrentSlide];
            TutorialImage.sprite = currentSlide.TutorialImage;
            TutorialText.text = currentSlide.TutorialText;
        }
        else
        {
            PlayerChannel.RaiseMovementEnabled(true);
            gameObject.SetActive(false);
        }
    }
}
