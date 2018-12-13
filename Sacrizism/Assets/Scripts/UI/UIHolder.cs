using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHolder : MonoBehaviour
{
    private const float waitTimeBase = 0.04f;

    public List<GameObject> elements;

    private int currentIndex = 0;
    private bool skipToNextStep = false;
    private bool isPlaying = false;

    private void Update()
    {
        if(isPlaying && Input.anyKeyDown)
        {
            skipToNextStep = true;
        }
    }

    public IEnumerator PlayBlock()
    {
        isPlaying = true;
        float nextWaitTime = GetNextElementWaitTime();
        float waitTimePassed = 0f;

        while(nextWaitTime >= 0f)
        {
            nextWaitTime = GetNextElementWaitTime();
            DisplayNextElement();            
            waitTimePassed = 0f;

            while(waitTimePassed < nextWaitTime)
            {
                if(skipToNextStep)
                {
                    skipToNextStep = false;
                    break;
                }
                waitTimePassed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

    public float GetNextElementWaitTime()
    {
        float waitTime = waitTimeBase;

        if (elements != null)
        {
            if (currentIndex < elements.Count)
            {
                Text nextText = elements[currentIndex].GetComponentInChildren<Text>();
                if (nextText)
                {
                    waitTime *= nextText.text.Length;
                }

                UIElement uiElement = elements[currentIndex].GetComponent<UIElement>();

                if(uiElement)
                {
                    if(uiElement.cancelWaitTime)
                    {
                        waitTime = waitTimeBase;
                    }
                    else
                    {
                        waitTime += uiElement.additionalWaitTime;
                    }
                }
            }
            else
            {
                waitTime = -1f;
            }
        }

        return waitTime;
    } 

    public void DisplayNextElement()
    {
        if(elements != null)
        {
            if(currentIndex < elements.Count)
            {
                GameManager.instance.audioManager.PlayUIElementAppearSound();
                
                elements[currentIndex].SetActive(true);

                currentIndex++;
            }
        }
    }
}
