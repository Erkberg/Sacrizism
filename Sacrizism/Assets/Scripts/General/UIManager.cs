using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image sacriBarFill;

    public GameObject powerUp;
    public Text powerUpText;

    public GameObject tutorial;
    public GameObject bossRedX;

    public GameObject blackBackground;
    public UIHolder introHolder;
    public UIHolder regularDeathHolder;
    public UIHolder bossDeathHolder;
    public UIHolder outroRegularHolder;
    public UIHolder creditsHolder;

    public Text restartFromBeginningText;
    public Text restartFromBossText;

    private bool waitingForSelection = false;
    // 0 - Restart from boss, 1 - Give up powers
    private int selectionID = 0;
    private int currentSelectionID = 0;

    private readonly Color selectedColor = Color.red;
    private readonly Color deselectedColor = new Color(1f, 0f, 0f, 0.33f);

    private void Awake()
    {
        powerUp.SetActive(false);
    }

    private void Update()
    {
        if(waitingForSelection)
        {
            CheckSelectionInput();
        }

        if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(PlayIntro());
    }

    private void CheckSelectionInput()
    {
        if(Input.GetButtonDown(InputConsts.SubmitButton))
        {
            if(selectionID == 0)
            {
                if(currentSelectionID == 0)
                {
                    GameManager.instance.RestartGame();
                }

                if (currentSelectionID == 1)
                {
                    GameManager.instance.RestartGame(true);
                }
            }
        }

        if(Input.GetAxis(InputConsts.VerticalMovementAxis) < 0f)
        {
            currentSelectionID = 1;
        }

        if (Input.GetAxis(InputConsts.VerticalMovementAxis) > 0f)
        {
            currentSelectionID = 0;
        }

        SetSelectionColors();
    }

    private void SetSelectionColors()
    {
        if (selectionID == 0)
        {
            if (currentSelectionID == 0)
            {
                restartFromBeginningText.color = selectedColor;
                restartFromBossText.color = deselectedColor;
            }

            if (currentSelectionID == 1)
            {
                restartFromBossText.color = selectedColor;
                restartFromBeginningText.color = deselectedColor;
            }
        }
    }

    public void SetSacriBarFillAmount(float amount)
    {
        sacriBarFill.fillAmount = amount;
    }

    public void OnPowerUpPickedUp(string text)
    {
        StopAllCoroutines();
        powerUp.SetActive(true);
        powerUpText.text = text;
        StartCoroutine(PowerUpSequence());
    }

    private IEnumerator PowerUpSequence()
    {
        yield return new WaitForSeconds(2f);
        powerUp.SetActive(false);
    }

    public void DisplayTutorial()
    {
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        tutorial.SetActive(true);
        yield return new WaitForSeconds(8f);
        tutorial.SetActive(false);
    }

    public void OnBossStarted()
    {
        bossRedX.SetActive(true);
    }

    public IEnumerator PlayIntro()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(introHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        introHolder.gameObject.SetActive(false);
        blackBackground.SetActive(false);

        GameManager.instance.OnIntroEnded();
    }

    public IEnumerator PlayRegularDeath()
    {
        yield return new WaitForSeconds(2f);
        blackBackground.SetActive(true);
        yield return StartCoroutine(regularDeathHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);

        GameManager.instance.RestartGame();
    }

    public IEnumerator PlayBossDeath()
    {
        blackBackground.SetActive(true);
        selectionID = 0;
        SetSelectionColors();
        yield return StartCoroutine(bossDeathHolder.PlayBlock());        
        waitingForSelection = true;
    }

    public IEnumerator PlayOutroRegular()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(outroRegularHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        outroRegularHolder.gameObject.SetActive(false);
        StartCoroutine(PlayCredits());
    }

    public IEnumerator PlayCredits()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(creditsHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        GameManager.instance.RestartGame();
    }

    public void OnClickRestartRegularButton()
    {
        GameManager.instance.RestartGame();
    }

    public void OnClickRestartBossButton()
    {
        GameManager.instance.RestartGame(true);
    }
}
