using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float blinktime;
    public ExclamationCanvas excCanvas;
    public string[] EventText;
    public string[] TaskText;
    public string[] AchievementText;
    public GameObject AchievementWinObj;
    public Text EventWindow;
    public Text TaskWindow;
    public Text AchievementWindow;
    public GameObject[] flag;
    public Slider ProgressBar;
    public Slider TreeAndGlass;
    private float ProgressValue;
    public float ProgressSpeed;
    public GameObject[] flags;
    public bool[] flagNotActive;
    public float FlagMoveDistance;
    public float FlagMoveSpeed;

    //mail
    public GameObject mailRedPoint;
    
    private void Start()
    {
        ProgressBar.value = 0;
        TreeAndGlass.value = 0;
        ProgressValue = 0;
    }
    public void OnPlantTreeClick()
    {
        Debug.Log("plantTree!");
    }
    public void exclamationBlink()
    {
        excCanvas.canblinking = true;
    }
    public void exclamationNotBlink()
    {
        excCanvas.canblinking = false;
    }

    public void ShowEventMessage(int num)
    {
        EventWindow.text = EventText[num];
    }
    public void ShowTaskMessage(int num)
    {
        TaskWindow.text = TaskText[num];
    }
    public void ProgressBarAdd()
    {
        ProgressValue += 0.166666666f;
    }
    public void ShowAChievementWindow(int num)
    {
        AchievementWinObj.SetActive(true);
        AchievementWindow.text = AchievementText[num];
    }
    public void OnQuitClick()
    {
        AchievementWinObj.SetActive(false);
    }

    //mail
    public void GetNewMail()
    {
        mailRedPoint.SetActive(true);
    }

    private void Update()
    {
        if(ProgressBar.value<ProgressValue)
        {
            ProgressBar.value += ProgressSpeed * Time.deltaTime;
        }
        if (TreeAndGlass.value < ProgressValue)
        {
            TreeAndGlass.value += ProgressSpeed * Time.deltaTime;
        }
        for(int i=1;i<7;i++)
        {
            if (Mathf.Abs(ProgressBar.value - 0.166666f * i) < 0.01) 
        {
               // Debug.Log(Mathf.Abs(ProgressBar.value - (1 / 6) * i));
                Debug.Log("cao");
            if(flagNotActive[i-1])
            {
               flagNotActive[i - 1] = false;
               flags[i - 1].SetActive(true);
               //flags[i - 1].GetComponent<FlagSet>().flagSet = true;
               flags[i - 1].GetComponent<FlagSet>().Movedistance = FlagMoveDistance;
               flags[i - 1].GetComponent<FlagSet>().moveSpeed = FlagMoveSpeed;
            }
        }
        }
        // Some Code for Test
        if (Input.GetKeyDown(KeyCode.A))
        {
            ProgressBarAdd();
        } 
        if(Input.GetKeyDown(KeyCode.B))
        {
            GetNewMail();
        }
    }
}
