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
		pcvr.GetInstance();
		JiaMiJiaoYanCtrlObj.SetActive(IsJiaMiTest);
        InputEventCtrl.GetInstance().ClickPcvrBtEvent01 += ClickPcvrBtEvent01;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent02 += ClickPcvrBtEvent02;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent03 += ClickPcvrBtEvent03;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent04 += ClickPcvrBtEvent04;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent05 += ClickPcvrBtEvent05;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent06 += ClickPcvrBtEvent06;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent07 += ClickPcvrBtEvent07;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent08 += ClickPcvrBtEvent08;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent09 += ClickPcvrBtEvent09;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent10 += ClickPcvrBtEvent10;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent11 += ClickPcvrBtEvent11;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent12 += ClickPcvrBtEvent12;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent13 += ClickPcvrBtEvent13;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent14 += ClickPcvrBtEvent14;
        InputEventCtrl.GetInstance().ClickPcvrBtEvent15 += ClickPcvrBtEvent15;
    }

    public void CheckReadComMsg(byte[] buffer)
    {
        UpdateDianWeiQiDt(buffer);
        UpdateBiZhiDt(buffer[18], buffer[19]);
        UpdateBianMaQiLbDt(buffer);
        UpdateBiZhiPlayerInfo();
        UpdateCaiPiaoJiInfo(buffer[44], buffer[44]);
    }

    public UILabel[] CaiPiaoJiLbArray;
    /// <summary>
    /// 更新彩票机状态信息.
    /// </summary>
    void UpdateCaiPiaoJiInfo(byte caiPiaoPrintSt01, byte caiPiaoPrintSt02)
    {
        pcvrTXManage.CaiPiaoPrintState state01 = (pcvrTXManage.CaiPiaoPrintState)caiPiaoPrintSt01;
        switch (state01)
        {
            case pcvrTXManage.CaiPiaoPrintState.WuXiao:
                {
                    CaiPiaoJiLbArray[0].text = "无效";
                    break;
                }
            case pcvrTXManage.CaiPiaoPrintState.Failed:
                {
                    CaiPiaoJiLbArray[0].text = "失败";
                    break;
                }
            case pcvrTXManage.CaiPiaoPrintState.Succeed:
                {
                    CaiPiaoJiLbArray[0].text = "成功";
                    break;
                }
        }

        pcvrTXManage.CaiPiaoPrintState state02 = (pcvrTXManage.CaiPiaoPrintState)caiPiaoPrintSt02;
        switch (state02)
        {
            case pcvrTXManage.CaiPiaoPrintState.WuXiao:
                {
                    CaiPiaoJiLbArray[1].text = "无效";
                    break;
                }
            case pcvrTXManage.CaiPiaoPrintState.Failed:
                {
                    CaiPiaoJiLbArray[1].text = "失败";
                    break;
                }
            case pcvrTXManage.CaiPiaoPrintState.Succeed:
                {
                    CaiPiaoJiLbArray[1].text = "成功";
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

        if (pcvr.GetInstance().mPcvrTXManage.CaiPiaoCountPrint[(int)caiPiaoJi] <= 0)
        {
            pcvr.GetInstance().mPcvrTXManage.SetCaiPiaoPrintCmd(printCmd, caiPiaoJi, countCaiPiao);
        }
    }

    /// <summary>
    /// 更新电位器信息.
    /// DianWeiQiLb[x]: 0 1px, 1 1py.
    /// </summary>
    public UILabel[] DianWeiQiLb;
    void UpdateDianWeiQiDt(byte[] buffer)
    {
        DianWeiQiLb[0].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[0].ToString();
        DianWeiQiLb[1].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[1].ToString();
        DianWeiQiLb[2].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[2].ToString();
        DianWeiQiLb[3].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[3].ToString();
        DianWeiQiLb[4].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[4].ToString();
        DianWeiQiLb[5].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[5].ToString();
        DianWeiQiLb[6].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[6].ToString();
        DianWeiQiLb[7].text = pcvr.GetInstance().mPcvrTXManage.DianWeiQiDtArray[7].ToString();
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

    /// <summary>
    /// BianMaQiLb[x]: 0 编码器1, 1 编码器2.
    /// </summary>
    public UILabel[] BianMaQiLb;
    /// <summary>
    /// 更新编码器信息.
    /// </summary>
    void UpdateBianMaQiLbDt(byte[] buffer)
    {
        BianMaQiLb[0].text = buffer[30].ToString("X2");
        BianMaQiLb[1].text = buffer[31].ToString("X2");
    }
    
    void ClickPcvrBtEvent01(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt01, val);
    }
    void ClickPcvrBtEvent02(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt02, val);
    }
    void ClickPcvrBtEvent03(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt03, val);
    }
    void ClickPcvrBtEvent04(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt04, val);
    }
    void ClickPcvrBtEvent05(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt05, val);
    }
    void ClickPcvrBtEvent06(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt06, val);
    }
    void ClickPcvrBtEvent07(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt07, val);
    }
    void ClickPcvrBtEvent08(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt08, val);
    }
    void ClickPcvrBtEvent09(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt09, val);
    }
    void ClickPcvrBtEvent10(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt10, val);
    }
    void ClickPcvrBtEvent11(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt11, val);
    }
    void ClickPcvrBtEvent12(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt12, val);
    }
    void ClickPcvrBtEvent13(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt13, val);
    }
    void ClickPcvrBtEvent14(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt14, val);
    }
    void ClickPcvrBtEvent15(InputEventCtrl.ButtonState val)
    {
        UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex.bt15, val);
    }

    /// <summary>
    /// AnJianLb[x]: 0 按键1, 1 按键2.
    /// </summary>
    public UILabel[] AnJianLb;
    /// <summary>
    /// 更新按键状态.
    /// </summary>
    void UpdateAnJianLbInfo(pcvrTXManage.AnJianIndex indexAnJian, InputEventCtrl.ButtonState btState)
    {
        byte indexVal = (byte)indexAnJian;
        indexVal -= 1;
        switch (btState)
        {
            case InputEventCtrl.ButtonState.DOWN:
                {
                    AnJianLb[indexVal].text = "按下";
                    break;
                }
            case InputEventCtrl.ButtonState.UP:
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
		pcvr.GetInstance().mPcvrTXManage.SubPlayerCoin(1, pcvrTXManage.PlayerCoinEnum.player04);
    }

    /// <summary>
    /// 更新币值信息.
    /// </summary>
    void UpdateBiZhiPlayerInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            BiZhiPlayerLb[i].text = pcvr.GetInstance().mPcvrTXManage.PlayerCoinArray[i].ToString();
        }
    }

    /// <summary>
    /// 点击关闭按键.
    /// </summary>
	public void OnClickCloseAppBt()
	{
		Application.Quit();
	}
	
	public bool IsJiaMiTest = false;
	public GameObject JiaMiJiaoYanCtrlObj;
    public UILabel JiaMiJYLabel;
    public UILabel JiaMiJYMsg;
    bool IsOpenJiaMiJiaoYan;
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

	public UILabel[] LedLabel = new UILabel[32];
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
        if (indexVal < 1 || indexVal > 32)
        {
            UnityEngine.Debug.LogError("OnClickLedBt -> indexVal was wrong! indexVal " + indexVal);
            return;
        }

        int indexValTmp = indexVal - 1;
        pcvr.GetInstance().mPcvrTXManage.LedState[indexValTmp] = !pcvr.GetInstance().mPcvrTXManage.LedState[indexValTmp];
		switch (pcvr.GetInstance().mPcvrTXManage.LedState[indexValTmp]) {
		case true:
			LedLabel[indexValTmp].text = indexVal + "灯亮";
			break;

		case false:
			LedLabel[indexValTmp].text = indexVal + "灯灭";
			break;
		}
	}

    public UILabel[] JiDianQiLbArray;
    /// <summary>
    /// 点击继电器控制按键.
    /// </summary>
    public void OnClickJiDianQiBt(GameObject bt)
    {
        byte indexVal = 0;
        string lbHead = "";
        string btName = bt.name;
        switch (btName)
        {
            case "Button_01":
                {
                    indexVal = 0;
                    lbHead = "1继电器";
                    break;
                }
            case "Button_02":
                {
                    indexVal = 1;
                    lbHead = "2继电器";
                    break;
                }
        }

        switch (pcvr.GetInstance().mPcvrTXManage.JiDianQiCmdArray[indexVal])
        {
            case pcvrTXManage.JiDianQiCmd.Close:
                {
                    pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(indexVal, pcvrTXManage.JiDianQiCmd.Open);
                    JiDianQiLbArray[indexVal].text = lbHead + "打开";
                    break;
                }
            case pcvrTXManage.JiDianQiCmd.Open:
                {
                    pcvr.GetInstance().mPcvrTXManage.SetJiDianQiCmd(indexVal, pcvrTXManage.JiDianQiCmd.Close);
                    JiDianQiLbArray[indexVal].text = lbHead + "关闭";
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
}