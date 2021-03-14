using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
	public float blinktime;
	public ExclamationCanvas excCanvas;
	public string[] EventTitle;
	public string[] EventText;
	public string[] TaskTitle;
	public string[] TaskText;
	public string[] AchievementTitle;
	public string[] AchievementText;
	public string[] EndTurnText;
	public GameObject AchievementWinObj;
	public GameObject EndTurnObj;
	public Text EndTurnT;
	public Text EventWindow;
	public Text EventT;
	public Text TaskWindow;
	public Text TaskT;
	public Text AchievementWindow;
	public Text AchievementT;
	public Slider ProgressBar;
	public Slider TreeAndGlass;
	private float ProgressValue;
	public float ProgressSpeed;
	public GameObject[] flags;
	public bool[] flagNotActive;
	public float FlagMoveDistance;
	public float FlagMoveSpeed;
	public GameObject mailScroll;
	public string[] ToGe;
	public string[] ToDouYe;

	public int ToGeEmailNum;
	public int ToMeiEmailNum;
	public bool canShowEmail;//是否可以展示信件
	public GameObject EmailPaper;
	public Text EmailText;

	public GameObject EmailPaper1;
	public Text EmailText1;
	//mail
	public GameObject mailRedPoint;

	public Text plantNeedNum;
	public Text ControlNeedNum;
	public Text Totalnum;

	public Text calendarYear;
	public Text calendarSeason;

	public Text EndTurnEnergy;
	public Text EndTurnDesert;
	public Text EndTurnStroy;

	public GameObject getMoneyObject;
	public float dialogfadespeed;
	public float moneySpeed;
	public float Moneytargethigh;
	private void Start()
	{
		ProgressBar.value = 0;
		TreeAndGlass.value = 0;
		ProgressValue = 0;
		ToGeEmailNum = -1;
		ToMeiEmailNum = 0;
	}
	public void OnPlantTreeClick()
	{
		if (FindObjectOfType<GameFlowCtrler>().labourForce < (int)PlayerActions.ActionType.Plant)
		{
			Debug.LogWarning("Not enough labourforce.");
		}
		else
		{
			FindObjectOfType<PlayerMovement>().Move(PlayerActions.ActionType.Plant);
			Debug.Log("plantTree!");
		}
	}
	public void OnHandlingDesert()
	{
		if (FindObjectOfType<GameFlowCtrler>().labourForce < (int)PlayerActions.ActionType.DesertHandle)
		{
			Debug.LogWarning("Not enough!");
		}
		else
		{
			FindObjectOfType<PlayerMovement>().Move(PlayerActions.ActionType.DesertHandle);
			Debug.Log("Handle!");
		}
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
		EventT.text = EventTitle[num];
	}
	public void ShowTaskMessage(int num)
	{
		TaskWindow.text = TaskText[num];
		TaskT.text = TaskTitle[num];
	}
	public void ProgressBarAdd()
	{
		ProgressValue += 0.166666666f;
	}
	public void ShowAChievementWindow(int num)
	{
		AchievementWinObj.SetActive(true);
		AchievementWindow.text = AchievementText[num];
		AchievementT.text = AchievementTitle[num];
		Time.timeScale = 0;
	}

	public void OnAchievementQuitClick()
	{
		AchievementWinObj.SetActive(false);
		Time.timeScale = 1;
	}
	public void ShowEndTurnWindow( int addEnergynum  ,bool isDesert,int num)
    {
		EndTurnObj.SetActive(true);
		EndTurnEnergy.text = "+"+addEnergynum.ToString();
		EndTurnDesert.gameObject.SetActive(isDesert);
		EndTurnStroy.text = EndTurnText[num];
		Time.timeScale = 0;
    }
	public void OnEndTurnQuitClick()
    {
		Time.timeScale = 1;
		EndTurnObj.SetActive(false);

	}
	public void OnMailButtonClick()
	{
		if (canShowEmail)
		{
			AudioMgr.Instance.PlayFx(AudioFxType.OpenLetter);
			mailRedPoint.SetActive(false);
			EmailPaper.SetActive(true);
			EmailText.text = ToGe[ToGeEmailNum];
			canShowEmail = false;
			Time.timeScale = 0;
		}

	}
	public void OnMailQuitClick()
	{
		Time.timeScale = 1;
		EmailPaper.SetActive(false);
	}

	public void ShowSendEmail()
	{
		EmailPaper1.SetActive(true);
		EmailText1.text = ToDouYe[ToMeiEmailNum];
	}
	public void OnSendEmailClick()
	{
		EmailPaper1.SetActive(false);
		ToMeiEmailNum++;
	}
	//mail
	public void GetNewMail()
	{
		mailRedPoint.SetActive(true);
		ToGeEmailNum++;
		canShowEmail = true;
	}

	public void ChangePlantNeedNum(int num1)
    {
		plantNeedNum.text = num1.ToString();
    }
	public void ChangeControlNeedNum(int num1)
	{
		ControlNeedNum.text = num1.ToString();
	}
	public void ChangeTotalNum(int num1)
    {
		Totalnum.text= num1.ToString();
    }

	public void OnEndTurnButtonClick()
    {

    }

	public void changeCalendar(int Yearnum,string season)
    {
		calendarYear.text = "第" + changeNumToChinese(Yearnum) + "年";
		calendarSeason.text = season;
    }

	public void addEnergyEffect(int changeEnergy)
    {
		StartCoroutine(changeMoney(changeEnergy));
    }
	IEnumerator changeMoney(int money)//����ǿ�Ǯ���Ǽ���������Ǽ�Ǯ���ǼӼӣ�money��д���Ǹı���
	{
		getMoneyObject.SetActive(true);
		Vector3 v3 = getMoneyObject.GetComponent<RectTransform>().position;
		if (money > 0)
		{
			getMoneyObject.GetComponentInChildren<Text>().text = "+" + money;
		}
		else
		{
			getMoneyObject.GetComponentInChildren<Text>().text = "-" + (-1 * money);
		}

		while (getMoneyObject.GetComponent<RectTransform>().position.y - (v3.y + Moneytargethigh) < -3)
		{
			getMoneyObject.GetComponent<RectTransform>().position = Vector3.Lerp(getMoneyObject.GetComponent<RectTransform>().position, v3 + new Vector3(0, Moneytargethigh, 0), Time.deltaTime * 5);
			yield return null;
		}
		while (getMoneyObject.GetComponent<CanvasGroup>().alpha > 0.05)
		{
			getMoneyObject.GetComponent<CanvasGroup>().alpha -= dialogfadespeed * Time.deltaTime * moneySpeed;
			yield return null;
		}
		getMoneyObject.SetActive(false);
		getMoneyObject.GetComponent<RectTransform>().position = v3;
		getMoneyObject.GetComponent<CanvasGroup>().alpha = 1;

	}
	private void Update()
	{
		if (ProgressBar.value < ProgressValue)
		{
			ProgressBar.value += ProgressSpeed * Time.deltaTime;
		}
		if (TreeAndGlass.value < ProgressValue)
		{
			TreeAndGlass.value += ProgressSpeed * Time.deltaTime;
		}
		for (int i = 1; i < 7; i++)
		{
			if (Mathf.Abs(ProgressBar.value - 0.166666f * i) < 0.01)
			{
				// Debug.Log(Mathf.Abs(ProgressBar.value - (1 / 6) * i));
				Debug.Log("cao");
				if (flagNotActive[i - 1])
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
		if (Input.GetKeyDown(KeyCode.B))
		{
			GetNewMail();
		}
		if(Input.GetKeyDown(KeyCode.C))
        {
			addEnergyEffect(10);
        }
	}
	string changeNumToChinese(int num)
    {
		if(num==1)
        {
			return "一";
		}
		if (num == 2)
		{
			return "二";
		}
		if (num == 3)
		{
			return "三";
		}
		if (num == 4)
		{
			return "四";
		}
		if (num == 5)
		{
			return "五";
		}
		if (num == 6)
		{
			return "六";
		}
		if (num == 7)
		{
			return "七";
		}
		if (num == 8)
		{
			return "八";
		}
		if (num == 9)
		{
			return "九";
		}
        else
        {
			return null;
        }

	}
}
