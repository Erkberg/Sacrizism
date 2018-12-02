using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image sacriBarFill;

    public GameObject powerUp;
    public Text powerUpText;

    private void Awake()
    {
        powerUp.SetActive(false);
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
}
