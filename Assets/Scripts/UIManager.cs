using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager instance;

    [Header("Main Menu")]
    [SerializeField] GameObject levelSelectorCanvas;
    [SerializeField] LevelButton[] levelButtons;
    [SerializeField] GameObject creditsCanvas;
    [SerializeField] AudioSource creditsSource;

    [Header("HUD")]
    [SerializeField] GameObject hudGameCanvas;
    [SerializeField] Image[] boomerangImage;
    [SerializeField] Image[] fruitImage;
    [SerializeField] GameObject chargeGroup;
    [SerializeField] Image chargeFill;

    [Header("End Gane Canvas")]
    [SerializeField] GameObject endGameCanvas;
    [SerializeField] TextMeshProUGUI endTitleText;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI endRecord;
    [SerializeField] Animator[] starsAnimators;

    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);
    }

    private void ResetHUD()
    {
        UpdateBoomerangsOnHud();

        UpdateFruitsOnHud();
    }

    public void UpdateChargeHud(float fill)
    {
        chargeGroup.SetActive(fill != 0);
        chargeFill.fillAmount = fill;
    }

    public void UpdateFruitsOnHud()
    {
        for (int i = 0; i < fruitImage.Length; i++)
        {
            if (i < GameManager.instance.FruitsCount)
                fruitImage[i].color = Color.white; 
            else
                fruitImage[i].color = Color.black;
        }
    }

    public void UpdateBoomerangsOnHud()
    {
        for (int i = 0; i < boomerangImage.Length; i++)
        {
            if (i < GameManager.instance.BoomerangCount)
                boomerangImage[i].color = Color.white;
            else
                boomerangImage[i].color = Color.black;
        }
    }

    public void ShowEndCanvas(string title, string score, int stars, bool isHighScore)
    {
        hudGameCanvas.SetActive(false);

        endTitleText.text = title;
        endScoreText.text = score.ToString();
        endRecord.gameObject.SetActive(isHighScore);

        for (int i = 0; i < starsAnimators.Length; i++)
        {
            if (i < stars)
                starsAnimators[i].GetComponent<Image>().color = Color.white;
            else
                starsAnimators[i].GetComponent<Image>().color = Color.gray;
        }

        endGameCanvas.SetActive(true);
    }

    public void ShowHUD()
    {
        ResetHUD();
        hudGameCanvas.SetActive(true);
    }

    public void ShowLevelSelector()
    {
        hudGameCanvas.SetActive(false);

        UpdateLevelSelector();

        levelSelectorCanvas.SetActive(true);
    }

    private void UpdateLevelSelector()
    {
        // Update levels info
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].GetComponent<Button>().interactable = GameManager.instance.levels[i].isUnlocked;
            for (int s = 0; s < levelButtons[i].stars.Length; s++)
            {
                if (s < GameManager.instance.levels[i].starCount)
                    levelButtons[i].stars[s].color = Color.white;
                else
                    levelButtons[i].stars[s].color = Color.gray;
            }
        }
    }

    public void ShowCredits()
    {
        hudGameCanvas.SetActive(false);

        UpdateLevelSelector();

        creditsCanvas.SetActive(true);

        creditsSource.Play();
    }
}
