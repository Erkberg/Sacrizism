using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image sacriBarFill;

    public void SetSacriBarFillAmount(float amount)
    {
        sacriBarFill.fillAmount = amount;
    }
}
