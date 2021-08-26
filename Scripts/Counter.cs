using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public ShinyHunt hunt;

    Text titleTextBox;
    Text counterTextBox;
    public string counterName;
    public int counterValue;
    public float totalTime = 0;
    public float averageTime = 0;

    void Start()
    {
        InitialiseTextBoxes();
    }

    public void InitialiseTextBoxes()
    {
        Text[] counterTextBoxes = gameObject.GetComponentsInChildren<Text>();
        foreach (Text text in counterTextBoxes)
        {
            if (text.gameObject.name == "Counter Title TextBox")
            {
                titleTextBox = text;
            }

            if (text.gameObject.name == "Counter Value TextBox")
            {
                counterTextBox = text;
            }
        }
    }

    public string SetCounterName(string counterName)
    {
        this.counterName = counterName;

        titleTextBox.text = counterName;

        return this.counterName;
    }

    public void SetCounterValue(int value)
    {
        counterValue = value;
        if (counterTextBox==null)
        {
            InitialiseTextBoxes();
        }
        counterTextBox.text = "" + counterValue;
    }

    public void SetCurrentTime(float time)
    {
        totalTime = time;
        averageTime = counterValue / time;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
    }

    public void IncrementCounter()
    {
        counterValue++;
        counterTextBox.text = "" + counterValue;

        for (int i = 0; i < hunt.counterTitles.Length; i++)
        {
            if (hunt.counterTitles[i] == counterName)
            {
                hunt.counterValues[i] = counterValue;
                hunt.Save();
                break;
            }
        }
    }

    public void DecrementCounter()
    {
        if (counterValue > 0)
        {
            counterValue--;
            counterTextBox.text = "" + counterValue;

            for (int i = 0; i < hunt.counterTitles.Length; i++)
            {
                if (hunt.counterTitles[i] == counterName)
                {
                    hunt.counterValues[i] = counterValue;
                    hunt.Save();
                    break;
                }
            }
        }
    }

    public void ResetCounter()
    {
        counterValue = 0;
        counterTextBox.text = "" + counterValue;

        for (int i = 0; i < hunt.counterTitles.Length; i++)
        {
            if (hunt.counterTitles[i] == counterName)
            {
                hunt.counterValues[i] = counterValue;
                hunt.Save();
                break;
            }
        }
    }
}
