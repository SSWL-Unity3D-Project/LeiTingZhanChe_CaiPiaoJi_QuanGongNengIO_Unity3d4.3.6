using UnityEngine;
using System;
using System.Diagnostics;

public class HardWareTest : MonoBehaviour
{
	static HardWareTest Instance;
    public static HardWareTest GetInstance()
    {
        return Instance;
    }

    void Start ()
	{
		Instance = this;
		JiaMiJiaoYanCtrlObj.SetActive(IsJiaMiTest);
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent01 += ClickPcvrBtEvent01;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent02 += ClickPcvrBtEvent02;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent03 += ClickPcvrBtEvent03;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent04 += ClickPcvrBtEvent04;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent05 += ClickPcvrBtEvent05;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent06 += ClickPcvrBtEvent06;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent07 += ClickPcvrBtEvent07;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent08 += ClickPcvrBtEvent08;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent09 += ClickPcvrBtEvent09;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent10 += ClickPcvrBtEvent10;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent11 += ClickPcvrBtEvent11;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent12 += ClickPcvrBtEvent12;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent13 += ClickPcvrBtEvent13;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent14 += ClickPcvrBtEvent14;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent15 += ClickPcvrBtEvent15;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent16 += ClickPcvrBtEvent16;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent17 += ClickPcvrBtEvent17;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent18 += ClickPcvrBtEvent18;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent19 += ClickPcvrBtEvent19;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent20 += ClickPcvrBtEvent20;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent21 += ClickPcvrBtEvent21;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent22 += ClickPcvrBtEvent22;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent23 += ClickPcvrBtEvent23;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent24 += ClickPcvrBtEvent24;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent25 += ClickPcvrBtEvent25;
        PcvrComInputEvent.GetInstance().ClickPcvrBtEvent26 += ClickPcvrBtEvent26;
    }

    public void CheckReadComMsg(byte[] buffer)
    {
        UpdateBiZhiDt(buffer[18], buffer[19]);
        UpdateBiZhiPlayerInfo();
        UpdateCaiPiaoJiInfo(buffer[44], buffer[15], buffer[16]);
    }

    public UILabel[] CaiPiaoJiLbArray;
    /// <summary>
    /// 更新彩票机状态信息.
    /// </summary>
    void UpdateCaiPiaoJiInfo(byte caiPiaoPrintSt01, byte caiPiaoPrintSt02, byte caiPiaoPrintSt03)
    {
        ShowCaiPiaoJiPrintState(0, (pcvrTXManage.CaiPiaoPrintState)caiPiaoPrintSt01);
        ShowCaiPiaoJiPrintState(1, (pcvrTXManage.CaiPiaoPrintState)caiPiaoPrintSt02);
        ShowCaiPiaoJiPrintState(2, (pcvrTXManage.CaiPiaoPrintState)caiPiaoPrintSt03);
    }

    /// <summary>
    /// 展示彩票机打印状态.
    /// </summary>
    void ShowCaiPiaoJiPrintState(int index, pcvrTXManage.CaiPiaoPrintState type)
    {
        switch (type)
        {
            case pcvrTXManage.CaiPiaoPrintState.WuXiao:
                {
                    CaiPiaoJiLbArray[index].text = "无效";
                    break;
                }
            case pcvrTXManage.CaiPiaoPrintState.Failed:
                {
                    CaiPiaoJiLbArray[index].text = "失败";
                    break;
                }
            case pcvrTXManage.CaiPiaoPrintState.Succeed:
                {
                    CaiPiaoJiLbArray[index].text = "成功";
                    break;
                }
        }
    }

    /// <summary>
    /// 点击打印彩票按键.
    /// </summary>
    public void OnClickPrintCaiPiao(GameObject btGroup, GameObject btPrint)
    {
        string btGroupName = btGroup.name;
        string btPrintName = btPrint.name;
        pcvrTXManage.CaiPiaoJi caiPiaoJi = pcvrTXManage.CaiPiaoJi.Null;
        pcvrTXManage.CaiPiaoPrintCmd printCmd = pcvrTXManage.CaiPiaoPrintCmd.WuXiao;
        switch (btGroupName)
        {
            case "caiPiaoJi01":
                {
                    caiPiaoJi = pcvrTXManage.CaiPiaoJi.Num01;
                    break;
                }
            case "caiPiaoJi02":
                {
                    caiPiaoJi = pcvrTXManage.CaiPiaoJi.Num02;
                    break;
                }
            case "caiPiaoJi03":
                {
                    caiPiaoJi = pcvrTXManage.CaiPiaoJi.Num03;
                    break;
                }
        }

        int countCaiPiao = 1;
        switch (btPrintName)
        {
            case "Button_01":
                {
                    printCmd = pcvrTXManage.CaiPiaoPrintCmd.QuanPiaoPrint;
                    break;
                }
            case "Button_02":
                {
                    printCmd = pcvrTXManage.CaiPiaoPrintCmd.BanPiaoPrint;
                    break;
                }
            case "Button_03":
                {
                    printCmd = pcvrTXManage.CaiPiaoPrintCmd.QuanPiaoPrint;
                    countCaiPiao = 5;
                    break;
                }
        }

        if (pcvr.GetInstance().mPcvrTXManage.GetIsCanPrintCaiPiao(caiPiaoJi))
        {
            pcvr.GetInstance().mPcvrTXManage.SetCaiPiaoPrintCmd(printCmd, caiPiaoJi, countCaiPiao);
        }
    }

    /// <summary>
    /// BiZhiLb[x]: 0 币值1, 1 币值2.
    /// </summary>
    public UILabel[] BiZhiLb;
    /// <summary>
    /// BiZhiPlayerLb[x]: 0 玩家1, 1 玩家2, 2 玩家3, 3 玩家4.
    /// </summary>
    public UILabel[] BiZhiPlayerLb;
    /// <summary>
    /// 更新币值信息.
    /// </summary>
    void UpdateBiZhiDt(byte biZhi01, byte biZhi02)
    {
        BiZhiLb[0].text = biZhi01.ToString("X2");
        BiZhiLb[1].text = biZhi02.ToString("X2");
    }
    
    void ClickPcvrBtEvent01(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt01, val);
    }
    void ClickPcvrBtEvent02(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt02, val);
    }
    void ClickPcvrBtEvent03(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt03, val);
    }
    void ClickPcvrBtEvent04(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt04, val);
    }
    void ClickPcvrBtEvent05(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt05, val);
    }
    void ClickPcvrBtEvent06(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt06, val);
    }
    void ClickPcvrBtEvent07(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt07, val);
    }
    void ClickPcvrBtEvent08(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt08, val);
    }
    void ClickPcvrBtEvent09(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt09, val);
    }
    void ClickPcvrBtEvent10(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt10, val);
    }
    void ClickPcvrBtEvent11(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt11, val);
    }
    void ClickPcvrBtEvent12(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt12, val);
    }
    void ClickPcvrBtEvent13(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt13, val);
    }
    void ClickPcvrBtEvent14(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt14, val);
    }
    void ClickPcvrBtEvent15(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt15, val);
    }
    void ClickPcvrBtEvent16(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt16, val);
    }
    void ClickPcvrBtEvent17(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt17, val);
    }
    void ClickPcvrBtEvent18(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt18, val);
    }
    void ClickPcvrBtEvent19(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt19, val);
    }
    void ClickPcvrBtEvent20(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt20, val);
    }
    void ClickPcvrBtEvent21(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt21, val);
    }
    void ClickPcvrBtEvent22(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt22, val);
    }
    void ClickPcvrBtEvent23(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt23, val);
    }
    void ClickPcvrBtEvent24(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt24, val);
    }
    void ClickPcvrBtEvent25(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt25, val);
    }
    void ClickPcvrBtEvent26(pcvr.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt26, val);
    }

    /// <summary>
    /// AnJianLb[x]: 0 按键1, 1 按键2.
    /// </summary>
    public UILabel[] AnJianLb;
    /// <summary>
    /// 更新按键状态.
    /// </summary>
    void UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex indexAnJian, pcvr.ButtonState btState)
    {
        byte indexVal = (byte)indexAnJian;
        indexVal -= 1;
        switch (btState)
        {
            case pcvr.ButtonState.DOWN:
                {
                    AnJianLb[indexVal].text = "按下";
                    break;
                }
            case pcvr.ButtonState.UP:
                {
                    AnJianLb[indexVal].text = "弹起";
                    break;
                }
        }
    }
        
    /// <summary>
    /// 点击减币按键.
    /// </summary>
	public void OnClickSubCoinBt()
	{
		pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(1, pcvrTXManage.PlayerCoinEnum.player01);
		pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(1, pcvrTXManage.PlayerCoinEnum.player02);
		pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(1, pcvrTXManage.PlayerCoinEnum.player03);
		//pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(1, pcvrTXManage.PlayerCoinEnum.player04);
    }

    /// <summary>
    /// 更新币值信息.
    /// </summary>
    void UpdateBiZhiPlayerInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            if (BiZhiPlayerLb.Length > i && BiZhiPlayerLb[i] != null)
            {
                BiZhiPlayerLb[i].text = pcvr.GetInstance().mPcvrTXManage.PlayerCoinArray[i].ToString();
            }
        }
    }

    /// <summary>
    /// 点击关闭按键.
    /// </summary>
	public void OnClickCloseAppBt()
	{
		Application.Quit();
	}
	
    /// <summary>
    /// 点击重启按键.
    /// </summary>
	public void OnClickRestartAppBt()
	{
        try
        {
            Application.Quit();
            RunCmd("start ComTest.exe");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("OnClickRestartAppBt::ex -> " + ex);
        }
	}
	void RunCmd(string command)
	{
		//實例一個Process類，啟動一個獨立進程    
		Process p = new Process();    //Process類有一個StartInfo屬性，這個是ProcessStartInfo類，    
		//包括了一些屬性和方法，下面我們用到了他的幾個屬性：   
		p.StartInfo.FileName = "cmd.exe";           //設定程序名   
		p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數   
		p.StartInfo.UseShellExecute = false;        //關閉Shell的使用    p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入    p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出   
		p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出    
		p.StartInfo.CreateNoWindow = true;          //設置不顯示窗口    
		p.Start();   //啟動
		
		//p.WaitForInputIdle();
		//MoveWindow(p.MainWindowHandle, 1000, 10, 300, 200, true);
		
		//p.StandardInput.WriteLine(command); //也可以用這種方式輸入要執行的命令    
		//p.StandardInput.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機    return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
	}

	public UILabel[] LedLabel = new UILabel[9];
    /// <summary>
    /// 点击led灯控制按键.
    /// </summary>
	public void OnClickLedBt(string parentName, string selfName)
	{
        if (!pcvr.bIsHardWare)
        {
            return;
        }

        int parentIndex = Convert.ToInt32(parentName.Substring(parentName.Length - 2, 2));
        int selfIndex = Convert.ToInt32(selfName.Substring(selfName.Length - 2, 2));
        int indexVal = ((parentIndex - 1) * 8) + selfIndex;
        if (indexVal < 1 || indexVal > 9)
        {
            UnityEngine.Debug.LogError("OnClickLedBt -> indexVal was wrong! indexVal " + indexVal);
            return;
        }

        if (indexVal < 1 || indexVal > 6)
        {
            //无效的led.
            //UnityEngine.Debug.LogWarning("OnClickLedBt -> the led wuxiao! indexVal ======= " + indexVal);
            return;
        }
        //UnityEngine.Debug.Log("OnClickLedBt -> indexVal ======= " + indexVal);

        /** *****************************************************************************************
         输出灯1 ---------- 1
         输出灯2 ---------- 2
         输出灯3 ---------- 3
         开始灯1 ---------- 4
         开始灯2 ---------- 5
         开始灯3 ---------- 6
         彩票灯1 ---------- ?
         彩票灯2 ---------- ?
         彩票灯3 ---------- ?
         ***************************************************************************************** */
        int indexValLed = indexVal - 1;
        pcvr.GetInstance().mPcvrTXManage.LedState[indexValLed] = !pcvr.GetInstance().mPcvrTXManage.LedState[indexValLed];
        
        int indexValTmp = indexVal - 1;
        string ledLbText = LedLabel[indexValTmp].text;
        ledLbText = ledLbText.Substring(0, ledLbText.Length - 1);
        switch (pcvr.GetInstance().mPcvrTXManage.LedState[indexValLed])
        {
            case true:
                {
                    LedLabel[indexValTmp].text = ledLbText + "亮";
                    break;
                }

            case false:
                {
                    LedLabel[indexValTmp].text = ledLbText + "灭";
                    break;
                }
        }
	}
    
    /// <summary>
    /// 级联led.
    /// </summary>
    public UILabel[] JiLianLedLabel = new UILabel[24];
    /// <summary>
    /// 点击led灯控制按键.
    /// </summary>
	public void OnClickJiLianLedBt(GameObject parentName, GameObject selfName)
    {
        if (!pcvr.bIsHardWare)
        {
            return;
        }

        int parentIndex = Convert.ToInt32(parentName.name.Substring(parentName.name.Length - 2, 2));
        int selfIndex = Convert.ToInt32(selfName.name.Substring(selfName.name.Length - 2, 2));
        int indexVal = ((parentIndex - 1) * 8) + selfIndex;
        if (indexVal < 9 || indexVal > 32)
        {
            UnityEngine.Debug.LogError("OnClickJiLianLedBt -> indexVal was wrong! indexVal " + indexVal);
            return;
        }
        //UnityEngine.Debug.Log("OnClickJiLianLedBt -> indexVal ======= " + indexVal);

        int indexValLed = indexVal - 1;
        pcvr.GetInstance().mPcvrTXManage.LedState[indexValLed] = !pcvr.GetInstance().mPcvrTXManage.LedState[indexValLed];

        int indexValTmp = indexVal - 9;
        string ledLbText = JiLianLedLabel[indexValTmp].text;
        ledLbText = ledLbText.Substring(0, ledLbText.Length - 1);
        switch (pcvr.GetInstance().mPcvrTXManage.LedState[indexValLed])
        {
            case true:
                {
                    JiLianLedLabel[indexValTmp].text = ledLbText + "亮";
                    break;
                }

            case false:
                {
                    JiLianLedLabel[indexValTmp].text = ledLbText + "灭";
                    break;
                }
        }
    }

    void CloseJiaMiJiaoYanFailed()
	{
		if (!IsInvoking("JiaMiJiaoYanFailed")) {
			return;
		}
		CancelInvoke("JiaMiJiaoYanFailed");
	}

    #region 加密芯片校验
    public bool IsJiaMiTest = false;
    public GameObject JiaMiJiaoYanCtrlObj;
    public UILabel JiaMiJYLabel;
    public UILabel JiaMiJYMsg;
    bool IsOpenJiaMiJiaoYan;
    public void OnClickJiaMiJiaoYanBt()
	{
		if (!IsOpenJiaMiJiaoYan) {
			UnityEngine.Debug.Log("OnClickJiaMiJiaoYanBt...");
			OpenJiaMiJiaoYan();
			JiaMiJYLabel.text = "开启校验";
			SetJiaMiJYMsg("校验中...", pcvrTXManage.JIAOYANENUM.NULL);
		}
	}
	
	public void OpenJiaMiJiaoYan()
	{
		if (IsOpenJiaMiJiaoYan) {
			return;
		}
		IsOpenJiaMiJiaoYan = true;
		pcvr.GetInstance().mPcvrTXManage.StartJiaoYanIO();
	}
	
	public void DelayCloseJiaMiJiaoYan()
	{
		CloseJiaMiJiaoYanFailed();
		Invoke("JiaMiJiaoYanFailed", 5f);
	}
	
	public void JiaMiJiaoYanFailed()
	{
		SetJiaMiJYMsg("", pcvrTXManage.JIAOYANENUM.FAILED);
	}

	public void JiaMiJiaoYanSucceed()
	{
		SetJiaMiJYMsg("", pcvrTXManage.JIAOYANENUM.SUCCEED);
	}
	
	public void CloseJiaMiJiaoYan()
	{
		if (!IsOpenJiaMiJiaoYan) {
			return;
		}
		IsOpenJiaMiJiaoYan = false;
	}
	
	void ResetJiaMiJYLabelInfo()
	{
		CloseJiaMiJiaoYan();
		JiaMiJYLabel.text = "加密校验";
	}
	
	public void SetJiaMiJYMsg(string msg, pcvrTXManage.JIAOYANENUM key)
	{
		switch (key) {
		case pcvrTXManage.JIAOYANENUM.SUCCEED:
			CloseJiaMiJiaoYanFailed();
			JiaMiJYMsg.text = "校验成功";
			ResetJiaMiJYLabelInfo();
			//ScreenLog.Log("校验成功");
			break;
			
		case pcvrTXManage.JIAOYANENUM.FAILED:
			CloseJiaMiJiaoYanFailed();
			JiaMiJYMsg.text = "校验失败";
			ResetJiaMiJYLabelInfo();
			//ScreenLog.Log("校验失败");
			break;
			
		default:
			JiaMiJYMsg.text = msg;
			//ScreenLog.Log(msg);
			break;
		}
	}
    #endregion
}