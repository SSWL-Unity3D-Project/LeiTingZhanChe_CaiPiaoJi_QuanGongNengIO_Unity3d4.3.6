using UnityEngine;

/// <summary>
/// Pcvr串口通信输入事件响应.
/// </summary>
public class PcvrComInputEvent : MonoBehaviour
{
    static PcvrComInputEvent _Instance = null;
    public static PcvrComInputEvent GetInstance()
	{
		if(_Instance == null)
		{
			GameObject obj = new GameObject("_InputEventCtrl");
			_Instance = obj.AddComponent<PcvrComInputEvent>();
            pcvr.GetInstance();
        }
		return _Instance;
	}

    #region Player Coin Event
    /// <summary>
    /// 玩家投币事件.
    /// </summary>
    public delegate void PlayerInsertCoinEvent(pcvrTXManage.PlayerCoinEnum indexPlayerCoin, int coin);
    public event PlayerInsertCoinEvent OnPlayerInsertCoinEvent;
    /// <summary>
    /// 玩家投币时间响应.
    /// coin -> 玩家的总投币数.
    /// </summary>
    public void OnPlayerInsertCoin(pcvrTXManage.PlayerCoinEnum indexPlayerCoin, int coin)
    {
        if (OnPlayerInsertCoinEvent != null)
        {
            OnPlayerInsertCoinEvent(indexPlayerCoin, coin);
        }
    }
    #endregion

    #region CaiPiaoJi Event
    /// <summary>
    /// 彩票机无票事件.
    /// </summary>
    public delegate void CaiPiaoJiWuPiaoEvent(pcvrTXManage.CaiPiaoJi val);
    public event CaiPiaoJiWuPiaoEvent OnCaiPiaJiWuPiaoEvent;
    public void OnCaiPiaJiWuPiao(pcvrTXManage.CaiPiaoJi val)
    {
        if (OnCaiPiaJiWuPiaoEvent != null)
        {
            OnCaiPiaJiWuPiaoEvent(val);
        }
    }

    /// <summary>
    /// 彩票机出票响应事件.
    /// </summary>
    public delegate void CaiPiaoJiChuPiaoEvent(pcvrTXManage.CaiPiaoJi val);
    public event CaiPiaoJiChuPiaoEvent OnCaiPiaJiChuPiaoEvent;
    public void OnCaiPiaJiChuPiao(pcvrTXManage.CaiPiaoJi val)
    {
        if (OnCaiPiaJiChuPiaoEvent != null)
        {
            OnCaiPiaJiChuPiaoEvent(val);
        }
    }
    #endregion

    #region Click Button Event
    /// <summary>
    /// 按键响应事件.
    /// </summary>
    public delegate void EventHandel(pcvr.ButtonState val);
    public event EventHandel ClickPcvrBtEvent01;
    public void ClickPcvrBt01(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent01 != null)
        {
            ClickPcvrBtEvent01(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent02;
    public void ClickPcvrBt02(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent02 != null)
        {
            ClickPcvrBtEvent02(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent03;
    public void ClickPcvrBt03(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent03 != null)
        {
            ClickPcvrBtEvent03(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent04;
    public void ClickPcvrBt04(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent04 != null)
        {
            ClickPcvrBtEvent04(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent05;
    public void ClickPcvrBt05(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent05 != null)
        {
            ClickPcvrBtEvent05(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent06;
    public void ClickPcvrBt06(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent06 != null)
        {
            ClickPcvrBtEvent06(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent07;
    public void ClickPcvrBt07(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent07 != null)
        {
            ClickPcvrBtEvent07(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent08;
    public void ClickPcvrBt08(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent08 != null)
        {
            ClickPcvrBtEvent08(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent09;
    public void ClickPcvrBt09(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent09 != null)
        {
            ClickPcvrBtEvent09(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent10;
    public void ClickPcvrBt10(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent10 != null)
        {
            ClickPcvrBtEvent10(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent11;
    public void ClickPcvrBt11(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent11 != null)
        {
            ClickPcvrBtEvent11(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent12;
    public void ClickPcvrBt12(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent12 != null)
        {
            ClickPcvrBtEvent12(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent13;
    public void ClickPcvrBt13(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent13 != null)
        {
            ClickPcvrBtEvent13(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent14;
    public void ClickPcvrBt14(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent14 != null)
        {
            ClickPcvrBtEvent14(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent15;
    public void ClickPcvrBt15(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent15 != null)
        {
            ClickPcvrBtEvent15(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent16;
    public void ClickPcvrBt16(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent16 != null)
        {
            ClickPcvrBtEvent16(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent17;
    public void ClickPcvrBt17(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent17 != null)
        {
            ClickPcvrBtEvent17(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent18;
    public void ClickPcvrBt18(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent18 != null)
        {
            ClickPcvrBtEvent18(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent19;
    public void ClickPcvrBt19(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent19 != null)
        {
            ClickPcvrBtEvent19(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent20;
    public void ClickPcvrBt20(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent20 != null)
        {
            ClickPcvrBtEvent20(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent21;
    public void ClickPcvrBt21(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent21 != null)
        {
            ClickPcvrBtEvent21(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent22;
    public void ClickPcvrBt22(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent22 != null)
        {
            ClickPcvrBtEvent22(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent23;
    public void ClickPcvrBt23(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent23 != null)
        {
            ClickPcvrBtEvent23(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent24;
    public void ClickPcvrBt24(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent24 != null)
        {
            ClickPcvrBtEvent24(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent25;
    public void ClickPcvrBt25(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent25 != null)
        {
            ClickPcvrBtEvent25(val);
        }
    }
    public event EventHandel ClickPcvrBtEvent26;
    public void ClickPcvrBt26(pcvr.ButtonState val)
    {
        if (ClickPcvrBtEvent26 != null)
        {
            ClickPcvrBtEvent26(val);
        }
    }
    #endregion
}