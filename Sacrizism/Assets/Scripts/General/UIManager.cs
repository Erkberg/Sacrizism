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

    private void Awake()
    {
        powerUp.SetActive(false);

        OnIntroEnded();
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

    public void OnIntroEnded()
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
}
