using UnityEngine;

public class pcvr : MonoBehaviour
{
    public enum ButtonState : int
    {
        UP = 1,
        DOWN = -1
    }
    /// <summary>
    /// 是否是硬件版.
    /// </summary>
    static public bool bIsHardWare = true;
    /// <summary>
    /// 是否校验hid.
    /// </summary>
	static public bool IsJiaoYanHid = false;
    /// <summary>
    /// pcvr通信数据管理.
    /// </summary>
    [HideInInspector]
    public pcvrTXManage mPcvrTXManage;
    static private pcvr Instance = null;
	static public pcvr GetInstance()
	{
		if (Instance == null) {
			GameObject obj = new GameObject("_PCVR");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<pcvr>();
            Instance.mPcvrTXManage = obj.AddComponent<pcvrTXManage>();
            if (bIsHardWare) {
				MyCOMDevice.GetInstance();
			}
		}
		return Instance;
    }
}