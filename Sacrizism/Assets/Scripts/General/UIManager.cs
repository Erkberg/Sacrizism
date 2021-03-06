﻿using System.Collections;
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
    public GameObject restartText;
    public GameObject sacribarHolder;

    public Image discoOverlay;
    public Color[] discoOverlayColors;

    public GameObject blackBackground;
    public UIHolder introHolder;
    public UIHolder regularDeathHolder;
    public UIHolder bossDeathHolder;
    public UIHolder bossIntroHolder;
    public UIHolder outroRegularHolder;
    public UIHolder creditsHolder;
    public UIHolder selfSacrificeHolder;
    public UIHolder bossIntroPacifistHolder;
    public UIHolder bossOutroPacifistHolder;
    public UIHolder bossMidtroTruePacifistHolder1;
    public UIHolder bossMidtroTruePacifistHolder2;
    public UIHolder bossOutroTruePacifistHolder;
    public UIHolder fullSacribarHolder;

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
        sacriBarFill.color = new Color(1f - amount, amount, 0f, 0.666f);
    }

    public void OnPowerUpPickedUp(string text)
    {
        StopAllCoroutines();
        powerUp.SetActive(true);
        powerUpText.text = text;
        tutorial.SetActive(false);
        StartCoroutine(PowerUpSequence());
    }

    private IEnumerator PowerUpSequence()
    {
        powerUp.transform.localScale = Vector3.zero;
        GameManager.instance.audioManager.PlayPowerUpSound();

        while(powerUp.transform.localScale.x < 1f)
        {
            powerUp.transform.localScale += Vector3.one * Time.deltaTime * 6f;
            yield return null;
        }

        powerUp.transform.localScale = Vector3.one;
        StartCoroutine(GameManager.instance.cameraMovement.ShortZoomOut());
        GameManager.instance.audioManager.PlayUIElementAppearSound();

        yield return new WaitForSeconds(2f);
        powerUp.SetActive(false);
    }

    public void DisplayTutorial(float displayTime)
    {
        StartCoroutine(TutorialSequence(displayTime));
    }

    private IEnumerator TutorialSequence(float displayTime)
    {
        tutorial.SetActive(true);
        yield return new WaitForSeconds(displayTime);
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
        yield return new WaitForSeconds(1f);
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

    public IEnumerator PlayOutroPacifist()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(bossOutroPacifistHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        bossOutroPacifistHolder.gameObject.SetActive(false);
        blackBackground.SetActive(false);
        GameManager.instance.OnPacifistEnding();
    }

    public IEnumerator PlayTruePacifistMidtro1()
    {
        blackBackground.SetActive(true);
        Time.timeScale = 0f;
        yield return StartCoroutine(bossMidtroTruePacifistHolder1.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        bossMidtroTruePacifistHolder1.gameObject.SetActive(false);
        blackBackground.SetActive(false);
        Time.timeScale = 1f;
    }

    public IEnumerator PlayTruePacifistMidtro2()
    {
        blackBackground.SetActive(true);
        Time.timeScale = 0f;
        yield return StartCoroutine(bossMidtroTruePacifistHolder2.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        bossMidtroTruePacifistHolder2.gameObject.SetActive(false);
        blackBackground.SetActive(false);
        Time.timeScale = 1f;
    }

    public IEnumerator PlayOutroTruePacifist()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(bossOutroTruePacifistHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        bossOutroTruePacifistHolder.gameObject.SetActive(false);
        blackBackground.SetActive(false);
        GameManager.instance.OnPacifistEnding();
    }

    public IEnumerator PlayOutroSelfSacrifice()
    {
        yield return new WaitForSeconds(2f);
        blackBackground.SetActive(true);
        yield return StartCoroutine(selfSacrificeHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        selfSacrificeHolder.gameObject.SetActive(false);
        StartCoroutine(PlayCredits());
    }

    public IEnumerator PlayOutroSacribarFull()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(fullSacribarHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        fullSacribarHolder.gameObject.SetActive(false);
        StartCoroutine(PlayCredits());
    }

    public IEnumerator PlayBossIntro()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(bossIntroHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        bossIntroHolder.gameObject.SetActive(false);
        blackBackground.SetActive(false);
    }

    public IEnumerator PlayBossIntroPacifist()
    {
        blackBackground.SetActive(true);
        yield return StartCoroutine(bossIntroPacifistHolder.PlayBlock());
        yield return new WaitUntil(() => Input.anyKeyDown);
        bossIntroPacifistHolder.gameObject.SetActive(false);
        blackBackground.SetActive(false);
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

    public IEnumerator ShowRestartDelayed()
    {
        yield return new WaitForSeconds(10f);
        restartText.SetActive(true);
    }

    public IEnumerator DiscoSequence()
    {
        sacribarHolder.SetActive(false);
        WaitForSeconds discoDelay = new WaitForSeconds(0.4f);
        discoOverlay.gameObject.SetActive(true);
        int previousIndex = 0;
        int newIndex = 0;

        while(true)
        {
            while(newIndex == previousIndex)
            {
                newIndex = Random.Range(0, discoOverlayColors.Length);
            }

            previousIndex = newIndex;
            discoOverlay.color = discoOverlayColors[newIndex];
            yield return discoDelay;
        }
    }
}
