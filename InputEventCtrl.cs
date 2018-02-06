using UnityEngine;
using System.Collections;

public class InputEventCtrl : MonoBehaviour {

	public static bool IsClickFireBtDown;
	public static uint SteerValCur;
	public static uint TaBanValCur;
	static private InputEventCtrl Instance = null;
	static public InputEventCtrl GetInstance()
	{
		if(Instance == null)
		{
			GameObject obj = new GameObject("_InputEventCtrl");
			Instance = obj.AddComponent<InputEventCtrl>();
		}
		return Instance;
	}

	#region Click Button Envent
	public delegate void EventHandel(ButtonState val);
	public event EventHandel ClickStartBtOneEvent;
	public void ClickStartBtOne(ButtonState val)
	{
		if(ClickStartBtOneEvent != null)
		{
			ClickStartBtOneEvent( val );
			//pcvr.StartBtLight = StartLightState.Mie;
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}

	public event EventHandel ClickCloseDongGanBtEvent;
	public void ClickCloseDongGanBt(ButtonState val)
	{
		if(ClickCloseDongGanBtEvent != null)
		{
			ClickCloseDongGanBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}
	
	public event EventHandel ClickPlayerYouMenBtEvent;
	public void ClickPlayerYouMenBt(ButtonState val)
	{
		if(ClickPlayerYouMenBtEvent != null)
		{
			ClickPlayerYouMenBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}

	public event EventHandel ClickSetEnterBtEvent;
	public void ClickSetEnterBt(ButtonState val)
	{
		if(ClickSetEnterBtEvent != null)
		{
			ClickSetEnterBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}

	public event EventHandel ClickSetMoveBtEvent;
	public void ClickSetMoveBt(ButtonState val)
	{
		if(ClickSetMoveBtEvent != null)
		{
			ClickSetMoveBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}

	public event EventHandel ClickFireBtEvent;
	public void ClickFireBt(ButtonState val)
	{
		if(ClickFireBtEvent != null)
		{
			ClickFireBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}
	
	public event EventHandel ClickShaCheBtEvent;
	public void ClickShaCheBt(ButtonState val)
	{
		if(ClickShaCheBtEvent != null)
		{
			ClickShaCheBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}
	
	public event EventHandel ClickLaBaBtEvent;
	public void ClickLaBaBt(ButtonState val)
	{
		if(ClickLaBaBtEvent != null)
		{
			ClickLaBaBtEvent( val );
		}
		pcvr.GetInstance().SetIsPlayerActivePcvr();
	}
	#endregion
}

public enum ButtonState : int
{
	UP = 1,
	DOWN = -1
}