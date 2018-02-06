#define SHI_ZI_QI_NANG

using UnityEngine;
using System.Collections;
using System;

public class pcvr : MonoBehaviour
{
    static public bool bIsHardWare = true;
	static public bool IsTestGame = false;
    /// <summary>
    /// 继电器命令.
    /// </summary>
    public enum JiDianQiCmd
    {
        Close,
        Open,
    }
    /// <summary>
    /// 继电器开关命令.
    /// </summary>
    public JiDianQiCmd[] JiDianQiCmdArray = new JiDianQiCmd[2];
    /// <summary>
    /// 彩票打印状态.
    /// </summary>
    public enum CaiPiaoPrintState
    {
        WuXiao = 0x00,
        Succeed = 0x55,
        Failed = 0xaa,
    }
    /// <summary>
    /// 彩票机编号.
    /// </summary>
    public enum CaiPiaoJi
    {
        Null = -1,
        Num01 = 0,
        Num02 = 1,
    }
    /// <summary>
    /// 彩票打印命令.
    /// </summary>
    public enum CaiPiaoPrintCmd
    {
        WuXiao = 0x00,
        QuanPiaoPrint = 0x55,
        BanPiaoPrint = 0x66,
        StopPrint = 0xaa,
    }
    CaiPiaoPrintCmd[] CaiPiaoPrintCmdVal = new CaiPiaoPrintCmd[2];
    public int[] CaiPiaoCountPrint = new int[2];
    CaiPiaoPrintState[] CaiPiaoJiPrintStArray = new CaiPiaoPrintState[2];
    //public static uint ShaCheCurPcvr;
    //static bool IsClickLaBaBt;
    static public uint gOldCoinNum = 0;
	static private uint mOldCoinNum = 0;
	public int CoinNumCurrent = 0;
	static public bool IsCloseDongGanBtDown = false;
	//static public bool bPlayerStartKeyDown = false;
	private bool bSetEnterKeyDown = false;
	static public bool bSetMoveKeyDown = false;
	public static bool IsZhenDongFangXiangPan;
	int SubCoinNum = 0;
	//public static bool m_IsOpneForwardQinang = false;
	//public static bool m_IsOpneBehindQinang = false;
	//public static bool m_IsOpneLeftQinang = false;
	//public static bool m_IsOpneRightQinang = false;
	//public static bool m_IsOpneQinang1 = false;
	//public static bool m_IsOpneQinang2 = false;
	//public static bool m_IsOpneQinang3 = false;
	//public static bool m_IsOpneQinang4 = false;
	public static uint SteerValMax = 999999;
	public static uint SteerValCen = 1765;
	public static uint SteerValMin = 0;
	public static uint SteerValCur;
	public static float mGetSteer = 0f;
	//public static uint BikeShaCheCur;
	//public static uint mBikePowerMin = 999999;
	//public static uint mBikePowerMax = 0;
	//public static float mGetPower = 0f;
	//static uint BikePowerLen = 0;
	//public static uint BikePowerCur;
	//public static uint BikePowerOld;
	bool bIsJiaoYanBikeValue = false;
	static bool IsInitYouMenJiaoZhun = false;
	//bool IsJiaoZhunFireBt;
	//bool IsFanZhuangYouMen;
	static bool IsInitFangXiangJiaoZhun;
	bool IsFanZhuangFangXiang;
	int FangXiangJiaoZhunCount;
	public static uint CoinCurPcvr;
	//public static uint BikePowerCurPcvr;
	//public static StartLightState StartBtLight = StartLightState.Mie;
	//public static StartLightState DongGanBtLight = StartLightState.Mie;
	bool IsCleanHidCoin;
	bool[] IsCleanHidCoinArray = new bool[4];
    static uint BikeDirLenA;
	static uint BikeDirLenB;
	static uint BikeDirLenC;
	static uint BikeDirLenD;
	//public static bool IsActiveSheCheEvent;
	//static bool IsInitShaCheJiaoZhun;
	//static bool IsFanZhuangShaChe;
	//static uint mBikeShaCheMin = 999999;
	//static uint mBikeShaCheMax = 0;
	//static uint BikeShaCheLen = 1;
	bool IsPlayFangXiangPanZhenDong;
	static private pcvr Instance = null;
	static public pcvr GetInstance()
	{
		if (Instance == null) {
			GameObject obj = new GameObject("_PCVR");
			DontDestroyOnLoad(obj);
			Instance = obj.AddComponent<pcvr>();
			if (bIsHardWare) {
				MyCOMDevice.GetInstance();
			}

			//ScreenLog.Log("open hid***********************");
		}
		return Instance;
	}

	static bool IsInitFangXiangPower;
	void InitFangXiangPowerOpen()
	{
		if (HardWareTest.IsTestHardWare) {
			return;
		}

		if (IsInitFangXiangPower) {
			return;
		}
		IsInitFangXiangPower = true;
		OpenFangXiangPanPower();
		//Debug.Log("*********");
		Invoke("DelayCloseFangXiangPanPower", 300f);
	}

	void DelayCloseFangXiangPanPower()
	{
		//Debug.Log("*********55555555555");
		IsInitFangXiangPower = false;
		if (Application.loadedLevel != 1) {
			CloseFangXiangPanPower();
		}
	}

	void Start()
	{
		InitJiaoYanMiMa();

		//FangXiangInfo
		SteerValMin = (uint)PlayerPrefs.GetInt("mBikeDirMin");
		SteerValCen = (uint)PlayerPrefs.GetInt("mBikeDirCen");
		SteerValMax = (uint)PlayerPrefs.GetInt("mBikeDirMax");
		CheckBikeDirLen();
		
		//YouMenInfo
		//mBikePowerMin = (uint)PlayerPrefs.GetInt("mBikePowerMin");
		//mBikePowerMax = (uint)PlayerPrefs.GetInt("mBikePowerMax");
		//BikePowerLen = mBikePowerMax < mBikePowerMin ? (mBikePowerMin - mBikePowerMax + 1) : (mBikePowerMax - mBikePowerMin + 1);
		//BikePowerLen = Math.Max(1, BikePowerLen);

		//mBikeShaCheMin = (uint)PlayerPrefs.GetInt("mBikeShaCheMin");
		//mBikeShaCheMax = (uint)PlayerPrefs.GetInt("mBikeShaCheMax");
		//BikeShaCheLen = mBikeShaCheMax < mBikeShaCheMin ? (mBikeShaCheMin - mBikeShaCheMax + 1) : (mBikeShaCheMax - mBikeShaCheMin + 1);
		//BikeShaCheLen = Math.Max(1, BikeShaCheLen);
		
		InitFangXiangPowerOpen();
	}

	void FixedUpdate()
	{
		if (IsTestGame  &&  Input.GetKeyUp(KeyCode.O)) {
			IsHandleDirByKey = !IsHandleDirByKey;
		}

		//GetPcvrPowerVal();
		GetPcvrSteerVal();
		//GetPcvrShaCheVal();
		if (!bIsHardWare) {
			return;
		}

        //减币，方向有问题.
        //机台动感有问题.
        //CRC校验不懂.
		SendMessage();
		GetMessage();
	}
	
	static byte ReadHead_1 = 0x53;
	static byte ReadHead_2 = 0x57;
    static byte EndRead_1 = 0x0d;
    static byte EndRead_2 = 0x0a;
    static byte WriteHead_1 = 0x09;
	static byte WriteHead_2 = 0x05;
	static byte WriteEnd_1 = 0x0d;
	static byte WriteEnd_2 = 0x0a;
	public static bool IsOpenFangXiangPanPower = true;
	//public static StartLightState ShaCheBtLight = StartLightState.Mie;
	public static void OpenFangXiangPanPower()
	{
		IsOpenFangXiangPanPower = true;
	}
	
	public static void CloseFangXiangPanPower()
	{
		if (IsInitFangXiangPower) {
			return;
		}
		IsOpenFangXiangPanPower = false;
	}

	void SendMessage()
	{
		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}

		byte[] buffer = new byte[MyCOMDevice.ComThreadClass.BufLenWrite];
		for (int i = 5; i < (MyCOMDevice.ComThreadClass.BufLenWrite - 2); i++) {
//			if (i >= 7 && i <= 11) {
//				continue;
//			}

//			if (i > 34 && i < 41) {
//				//方向盘信息.
//				continue;
//			}
			buffer[i] = (byte)UnityEngine.Random.Range(0x00, 0xff);
		}
		buffer[0] = WriteHead_1;
		buffer[1] = WriteHead_2;
		buffer[MyCOMDevice.ComThreadClass.BufLenWrite - 2] = WriteEnd_1;
		buffer[MyCOMDevice.ComThreadClass.BufLenWrite - 1] = WriteEnd_2;
		
		//switch (StartBtLight) {
		//case StartLightState.Liang:
		//	buffer[4] |= 0x40;
		//	break;
			
		//case StartLightState.Shan:
		//	buffer[4] |= 0x40;
		//	break;
			
		//case StartLightState.Mie:
		//	buffer[4] &= 0xbf;
		//	break;
		//}
		
		//switch (ShaCheBtLight) {
		//case StartLightState.Liang:
		//	buffer[7] = 0xaa;
		//	break;
			
		//case StartLightState.Shan:
		//	buffer[7] = 0x55;
		//	break;
			
		//case StartLightState.Mie:
		//	buffer[7] = 0x00;
		//	break;
		//}

		//if (TouBiInfoCtrl.IsCloseDongGan || TouBiInfoCtrl.IsCloseQiNang) {
		//	buffer[4] <<= 4;
		//}
		//else {
			/*
0	气囊1：充气1、放气0	（快艇气囊1）		0x01	0xFE
1	气囊2：充气1、放气0	（快艇气囊2）		0x02	0xFD
2	气囊3：充气1、放气0	（快艇气囊3）		0x04	0xFB
3	气囊4：充气1、放气0	（快艇气囊4）		0x08	0xF7

1    2

4    3
			 */

            /*
十字型气囊            
             
        1
    4       2
		3

             */

//#if SHI_ZI_QI_NANG

     //       if (!HardWareTest.IsTestHardWare || HardWareTest.m_IsHitshake)
     //       {
     //           if (SetPanel.IsOpenSetPanel)
     //           {
     //               if (m_IsOpneForwardQinang)
     //               {
     //                   buffer[4] |= 0x01;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xfe;
     //               }

					//if (m_IsOpneRightQinang)
					//{
					//	buffer[4] |= 0x02;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xfd;
     //               }

					//if (m_IsOpneBehindQinang)
     //               {
     //                   buffer[4] |= 0x04;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xfb;
     //               }

					//if (m_IsOpneLeftQinang)
     //               {
     //                   buffer[4] |= 0x08;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xf7;
     //               }

     //           }
     //           else
     //           {
     //               if (m_IsOpneForwardQinang)
     //               {
     //                   buffer[4] |= 0x01;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xfe;
     //               }

					//if (m_IsOpneRightQinang)
					//{
					//	buffer[4] |= 0x02;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xfd;
     //               }

					//if (m_IsOpneBehindQinang)
     //               {
     //                   buffer[4] |= 0x04;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xfb;
     //               }

					//if (m_IsOpneLeftQinang)
     //               {
     //                   buffer[4] |= 0x08;
     //               }
     //               else
     //               {
     //                   buffer[4] &= 0xf7;
     //               }
     //           }
     //       }
     //       else
     //       {
     //           if (m_IsOpneQinang1)
     //           {
     //               buffer[4] |= 0x01;
     //           }
     //           else
     //           {
     //               buffer[4] &= 0xfe;
     //           }

     //           if (m_IsOpneQinang2)
     //           {
     //               buffer[4] |= 0x02;
     //           }
     //           else
     //           {
     //               buffer[4] &= 0xfd;
     //           }

     //           if (m_IsOpneQinang3)
     //           {
     //               buffer[4] |= 0x04;
     //           }
     //           else
     //           {
     //               buffer[4] &= 0xfb;
     //           }

     //           if (m_IsOpneQinang4)
     //           {
     //               buffer[4] |= 0x08;
     //           }
     //           else
     //           {
     //               buffer[4] &= 0xf7;
     //           }
     //       }

//#else
//            if (!HardWareTest.IsTestHardWare || HardWareTest.m_IsHitshake) {
//				if (SetPanel.IsOpenSetPanel) {
//					if (m_IsOpneForwardQinang) {
//						buffer[4] |=  0x01;
//					}
//					else {
//						buffer[4] &=  0xfe;
//					}
					
//					if (m_IsOpneBehindQinang) {
//						buffer[4] |=  0x02;
//					}
//					else {
//						buffer[4] &=  0xfd;
//					}
					
//					if (m_IsOpneLeftQinang) {
//						buffer[4] |=  0x04;
//					}
//					else {
//						buffer[4] &=  0xfb;
//					}
					
//					if (m_IsOpneRightQinang) {
//						buffer[4] |=  0x08;
//					}
//					else {
//						buffer[4] &=  0xf7;
//					}
//				}
//				else {
//					if (m_IsOpneForwardQinang) {
//						buffer[4] |=  0x01;
//						buffer[4] |=  0x02;
//					}
//					else {
//						buffer[4] &=  0xfe;
//						buffer[4] &=  0xfd;
//					}
					
//					if (m_IsOpneBehindQinang) {
//						buffer[4] |=  0x04;
//						buffer[4] |=  0x08;
//					}
//					else {
//						buffer[4] &=  0xfb;
//						buffer[4] &=  0xf7;
//					}
					
//					if (m_IsOpneLeftQinang) {
//						buffer[4] |=  0x01;
//						buffer[4] |=  0x08;
//					}
//					else {
//						if (!m_IsOpneForwardQinang) {
//							buffer[4] &=  0xfe;
//							buffer[4] &=  0xf7;
//						}
//					}
					
//					if (m_IsOpneRightQinang) {
//						buffer[4] |=  0x02;
//						buffer[4] |=  0x04;
//					}
//					else {
//						if (!m_IsOpneForwardQinang) {
//							buffer[4] &=  0xfd;
//							buffer[4] &=  0xfb;
//						}
//					}
//				}
//			}
//			else {
				
//				if (m_IsOpneQinang1) {
//					buffer[4] |=  0x01;
//				}
//				else {
//					buffer[4] &=  0xfe;
//				}
				
//				if (m_IsOpneQinang2) {
//					buffer[4] |=  0x02;
//				}
//				else {
//					buffer[4] &=  0xfd;
//				}
				
//				if (m_IsOpneQinang3) {
//					buffer[4] |=  0x04;
//				}
//				else {
//					buffer[4] &=  0xfb;
//				}
				
//				if (m_IsOpneQinang4) {
//					buffer[4] |=  0x08;
//				}
//				else {
//					buffer[4] &=  0xf7;
//				}
//			}
//#endif
        //}

		//if (IsZhenDongFangXiangPan) {
		//	buffer[6] = 0x55;
		//}
		//else {
		//	if (IsOpenFangXiangPanPower) {
		//		buffer[6] = 0xaa;
		//	}
		//	else {
		//		buffer[6] = 0x00;
		//	}
		//}
        
        //减币控制.
        if (!IsCleanHidCoinArray[0] && !IsCleanHidCoinArray[1] && !IsCleanHidCoinArray[2] && !IsCleanHidCoinArray[3])
        {
            buffer[4] = 0x00;
        }
        else
        {
            if (IsCleanHidCoinArray[0] || IsCleanHidCoinArray[1])
            {
                buffer[4] = 0x04;
                if (IsCleanHidCoinArray[0] && IsCleanHidCoinArray[1])
                {
                    buffer[2] = 0x11;
                }
                else if (IsCleanHidCoinArray[0])
                {
                    buffer[2] = 0x01;
                }
                else if (IsCleanHidCoinArray[1])
                {
                    buffer[2] = 0x10;
                }
            }

            if (IsCleanHidCoinArray[2] || IsCleanHidCoinArray[3])
            {
                buffer[4] = 0x04;
                if (IsCleanHidCoinArray[2] && IsCleanHidCoinArray[3])
                {
                    buffer[3] = 0x11;
                }
                else if (IsCleanHidCoinArray[2])
                {
                    buffer[3] = 0x01;
                }
                else if (IsCleanHidCoinArray[3])
                {
                    buffer[3] = 0x10;
                }
            }
        }

  //      if (IsCleanHidCoin) {
		//	buffer[4] = 0x04;
		//	buffer[2] = 0x01;
		//	buffer[3] = 0x10;
		//	if (CoinCurPcvr == 0) {
		//		IsCleanHidCoin = false;
		//	}
		//}
		//else {
		//	buffer[4] = 0x00;
		//}

		//FangXiangPanInfo
//		buffer[35] = FangXiangPanL_1;
//		buffer[36] = FangXiangPanL_2;
//		buffer[37] = FangXiangPanR_1;
//		buffer[38] = FangXiangPanR_2;
//		buffer[39] = FangXiangPanM_1;
//		buffer[40] = FangXiangPanM_2;

		if (IsJiaoYanHid) {
			//for (int i = 0; i < 4; i++) {
			//	buffer[i + 10] = JiaoYanMiMa[i];
			//}

			//for (int i = 0; i < 4; i++) {
			//	buffer[i + 14] = JiaoYanDt[i];
			//}
		}
		else {
			//RandomJiaoYanMiMaVal();
			//for (int i = 0; i < 4; i++) {
			//	buffer[i + 10] = JiaoYanMiMaRand[i];
			//}

			////0x41 0x42 0x43 0x44
			//for (int i = 15; i < 18; i++) {
			//	buffer[i] = (byte)UnityEngine.Random.Range(0x00, 0x40);
			//}
			//buffer[14] = 0x00;
			
			//for (int i = 15; i < 18; i++) {
			//	buffer[14] ^= buffer[i];
			//}
		}

        //彩票打印控制.
        buffer[19] = (byte)CaiPiaoPrintCmdVal[(int)CaiPiaoJi.Num01];
        buffer[20] = (byte)CaiPiaoPrintCmdVal[(int)CaiPiaoJi.Num02];

        //灯1控制
        LedData ledDt = new LedData(LedIndexEnum.Led01, buffer[13], buffer[12], 0x02, 0x04, 0x40, 0x08);
        SetLedInfo(out buffer[13], out buffer[12], ledDt);

        //灯2控制
        ledDt = new LedData(LedIndexEnum.Led02, buffer[27], buffer[15], 0x04, 0x10, 0x40, 0x08);
        SetLedInfo(out buffer[27], out buffer[15], ledDt);

        //灯3控制
        ledDt = new LedData(LedIndexEnum.Led03, buffer[14], buffer[16], 0x04, 0x10, 0x04, 0x10);
        SetLedInfo(out buffer[14], out buffer[16], ledDt);

        //灯4控制
        ledDt = new LedData(LedIndexEnum.Led04, buffer[6], buffer[17], 0x04, 0x10, 0x04, 0x10);
        SetLedInfo(out buffer[6], out buffer[17], ledDt);

        //灯5控制
        ledDt = new LedData(LedIndexEnum.Led05, buffer[11], buffer[7], 0x04, 0x40, 0x80, 0x04);
        SetLedInfo(out buffer[11], out buffer[7], ledDt);

        //灯6控制
        ledDt = new LedData(LedIndexEnum.Led06, buffer[24], buffer[10], 0x02, 0x10, 0x04, 0x40);
        SetLedInfo(out buffer[24], out buffer[10], ledDt);

        //灯7控制
        ledDt = new LedData(LedIndexEnum.Led07, buffer[30], buffer[8], 0x40, 0x10, 0x20, 0x10);
        SetLedInfo(out buffer[30], out buffer[8], ledDt);

        //灯8控制
        ledDt = new LedData(LedIndexEnum.Led08, buffer[9], buffer[5], 0x02, 0x20, 0x20, 0x10);
        SetLedInfo(out buffer[9], out buffer[5], ledDt);

        //灯9控制
        SetLedState(LedIndexEnum.Led09, buffer[21], out buffer[21]);

        //灯10控制
        SetLedState(LedIndexEnum.Led10, buffer[21], out buffer[21]);

        //灯11控制
        SetLedState(LedIndexEnum.Led11, buffer[21], out buffer[21]);

        //灯12控制
        SetLedState(LedIndexEnum.Led12, buffer[21], out buffer[21]);

        //灯13控制
        SetLedState(LedIndexEnum.Led13, buffer[21], out buffer[21]);

        //灯14控制
        SetLedState(LedIndexEnum.Led14, buffer[21], out buffer[21]);

        //灯15控制
        SetLedState(LedIndexEnum.Led15, buffer[21], out buffer[21]);

        //灯16控制
        SetLedState(LedIndexEnum.Led16, buffer[21], out buffer[21]);

        //灯17控制
        SetLedState(LedIndexEnum.Led17, buffer[22], out buffer[22]);

        //灯18控制
        SetLedState(LedIndexEnum.Led18, buffer[22], out buffer[22]);

        //灯19控制
        SetLedState(LedIndexEnum.Led19, buffer[22], out buffer[22]);

        //灯20控制
        SetLedState(LedIndexEnum.Led20, buffer[22], out buffer[22]);

        //灯21控制
        SetLedState(LedIndexEnum.Led21, buffer[22], out buffer[22]);

        //灯22控制
        SetLedState(LedIndexEnum.Led22, buffer[22], out buffer[22]);

        //灯23控制
        SetLedState(LedIndexEnum.Led23, buffer[22], out buffer[22]);

        //灯24控制
        SetLedState(LedIndexEnum.Led24, buffer[22], out buffer[22]);

        //灯25控制
        SetLedState(LedIndexEnum.Led25, buffer[26], out buffer[26]);

        //灯26控制
        SetLedState(LedIndexEnum.Led26, buffer[26], out buffer[26]);

        //灯27控制
        SetLedState(LedIndexEnum.Led27, buffer[26], out buffer[26]);

        //灯28控制
        SetLedState(LedIndexEnum.Led28, buffer[26], out buffer[26]);

        //灯29控制
        SetLedState(LedIndexEnum.Led29, buffer[26], out buffer[26]);

        //灯30控制
        SetLedState(LedIndexEnum.Led30, buffer[26], out buffer[26]);

        //灯31控制
        SetLedState(LedIndexEnum.Led31, buffer[26], out buffer[26]);

        //灯32控制
        SetLedState(LedIndexEnum.Led32, buffer[26], out buffer[26]);

        //Led灯总控.
        SetLedZongKongInfo(out buffer[18], buffer[18]);

        //继电器控制.
        buffer[23] = GetJiDianQiCmd();

        buffer[25] = 0x85;
        for (int i = 2; i <= 35; i++)
        {
            if (i == 32 || i == 25)
            {
            }
            else
            {
                buffer[25] ^= buffer[i];
            }
        }

        buffer[32] = 0x58;
        for (int i = 9; i <= 28; i++)
        {
            buffer[32] ^= buffer[i];
        }

        buffer[46] = 0x53;
        for (int i = 33; i <= 49; i++)
        {
            if (i == 46)
            {
            }
            else
            {
                buffer[46] ^= buffer[i];
            }
        }
        MyCOMDevice.ComThreadClass.WriteByteMsg = buffer;
    }


    /*
    
#define User_KEY_2_Data_Buff	User_Usart_1_Tx_Buffer[27]
#define User_KEY_2_Rand_Buff	User_Usart_1_Tx_Buffer[15]

void User_KEY_2_Handle(void)			//coin 4 key 
{
	User_Key_RandData=(u8)(rand()%254+1);	
	
		if((User_Key_RandData&0x08)==0x08)		//      3
			{
				User_KEY_2_Rand_Buff=(u8)(rand()%254+1);
				User_KEY_2_Rand_Buff|=0x04;	    
				User_KEY_2_Rand_Buff&=0xef;					
				User_KEY_2_Data_Buff=(u8)(rand()%254+1);
					if(User_4P_Coin_Key_Real_State==0x0)	//°′??
					{
						User_KEY_2_Data_Buff&=0xdf;
					}
					else																	//μˉ?e
					{
						User_KEY_2_Data_Buff|=0x20;
					}
			}
		else                                    //5
			{
				User_KEY_2_Rand_Buff=(u8)(rand()%254+1);
				User_KEY_2_Rand_Buff|=0x10;	
				User_KEY_2_Rand_Buff&=0xfb;				//°′?üóDD§???aμú????
				User_KEY_2_Data_Buff=(u8)(rand()%254+1);
					if(User_4P_Coin_Key_Real_State==0x0)	//°′??
					{
						User_KEY_2_Data_Buff&=0xbf;
					}
					else																	//μˉ?e
					{
						User_KEY_2_Data_Buff|=0x40;
					}
			}
	User_Key_RandData=(u8)(rand()%254+1);
}
     */

    enum LedIndexEnum
    {
        Null = 0,
        Led01 = 1,
        Led02 = 2,
        Led03 = 3,
        Led04 = 4,
        Led05 = 5,
        Led06 = 6,
        Led07 = 7,
        Led08 = 8,
        Led09 = 9,
        Led10 = 10,
        Led11 = 11,
        Led12 = 12,
        Led13 = 13,
        Led14 = 14,
        Led15 = 15,
        Led16 = 16,
        Led17 = 17,
        Led18 = 18,
        Led19 = 19,
        Led20 = 20,
        Led21 = 21,
        Led22 = 22,
        Led23 = 23,
        Led24 = 24,
        Led25 = 25,
        Led26 = 26,
        Led27 = 27,
        Led28 = 28,
        Led29 = 29,
        Led30 = 30,
        Led31 = 31,
        Led32 = 32,
    }
    class LedData
    {
        public LedIndexEnum IndexLed = LedIndexEnum.Null;
        /// <summary>
        /// 有效检验数据
        /// </summary>
        public byte LedKey = 0;
        /// <summary>
        /// 有效数据
        /// </summary>
        public byte LedVal = 0;
        public byte LedKey01 = 0; //有效检验数1
        public byte LedKey02 = 0; //有效检验数2
        public byte LedVal01 = 0; //有效数据1
        public byte LedVal02 = 0; //有效数据2
        public LedData(LedIndexEnum indexLed, byte ledKey, byte ledVal, byte ledKey01, byte ledKey02, byte ledVal01, byte ledVal02)
        {
            IndexLed = indexLed;
            LedKey = ledKey;
            LedVal = ledVal;
            LedKey01 = ledKey01;
            LedKey02 = ledKey02;
            LedVal01 = ledVal01;
            LedVal02 = ledVal02;
        }
    }

    /// <summary>
    /// 设置Led(9-32)的状态.
    /// </summary>
    void SetLedState(LedIndexEnum indexLed, byte ledBuf, out byte outLedBuf)
    {
        bool isOpenLed = LedState[(int)indexLed - 1];
        int indexVal = ((int)indexLed - 1) % 8;
        int keyVal = (int)Mathf.Pow(2, indexVal);
        outLedBuf = (byte)(isOpenLed == true ? (ledBuf | keyVal) : (ledBuf & (~keyVal)));
    }

    /// <summary>
    /// 设置Led(1-8)的状态.
    /// </summary>
    void SetLedInfo(out byte ledKeyOut, out byte ledValOut, LedData ledDt)
    {
        byte indexLed = (byte)ledDt.IndexLed;
        indexLed -= 1;
        if (indexLed < 0 || indexLed > 7)
        {
            Debug.LogError("SetLedInfo -> indexLed was wrong! indexLed " + indexLed);
            ledKeyOut = ledDt.LedKey;
            ledValOut = ledDt.LedVal;
            return;
        }

        byte ledKey = ledDt.LedKey;
        byte ledVal = ledDt.LedVal;
        byte ledKey01 = ledDt.LedKey01; //有效检验数1
        byte ledKey02 = ledDt.LedKey02; //有效检验数2
        byte ledVal01 = ledDt.LedVal01; //有效数据1
        byte ledVal02 = ledDt.LedVal02; //有效数据2
        bool isOpenLed = LedState[indexLed];
        if (UnityEngine.Random.Range(0, 100) % 2 == 0)
        {
            ledKey |= ledKey01;
            ledKey = (byte)(ledKey & (~(ledKey02)));
            if (isOpenLed)
            {
                ledVal = (byte)(ledVal & (~(ledVal01)));
            }
            else
            {
                ledVal |= ledVal01;
            }
        }
        else
        {
            ledKey |= ledKey02;
            ledKey = (byte)(ledKey & (~(ledKey01)));
            if (isOpenLed)
            {
                ledVal = (byte)(ledVal & (~(ledVal02)));
            }
            else
            {
                ledVal |= ledVal02;
            }
        }

        ledKeyOut = ledKey;
        ledValOut = ledVal;
    }

    /// <summary>
    /// Led状态.
    /// </summary>
    public static bool[] LedState = new bool[32];
    /// <summary>
    /// 设置Led总控命令.
    /// </summary>
    void SetLedZongKongInfo(out byte ledZongKongNew, byte ledZongKongOld)
    {
        ledZongKongNew = ledZongKongOld;
        ledZongKongNew = (byte)(ledZongKongNew | 0x40);
        ledZongKongNew = (byte)(ledZongKongNew & 0xFB);
    }

    static void RandomJiaoYanDt()
	{	
		for (int i = 1; i < 4; i++) {
			JiaoYanDt[i] = (byte)UnityEngine.Random.Range(0x00, 0x7b);
		}
		JiaoYanDt[0] = 0x00;
		for (int i = 1; i < 4; i++) {
			JiaoYanDt[0] ^= JiaoYanDt[i];
		}
	}

	public void StartJiaoYanIO()
	{
		if (IsJiaoYanHid) {
			return;
		}

		if (!HardWareTest.IsTestHardWare) {
			if (JiaoYanSucceedCount >= JiaoYanFailedMax) {
				return;
			}
			
			if (JiaoYanState == JIAOYANENUM.FAILED && JiaoYanFailedCount >= JiaoYanFailedMax) {
				return;
			}
		}
		RandomJiaoYanDt();
		IsJiaoYanHid = true;
		CancelInvoke("CloseJiaoYanIO");
		Invoke("CloseJiaoYanIO", 5f);
		//ScreenLog.Log("开始校验...");
	}

	void CloseJiaoYanIO()
	{
		if (!IsJiaoYanHid) {
			return;
		}
		IsJiaoYanHid = false;
		OnEndJiaoYanIO(JIAOYANENUM.FAILED);

		if (HardWareTest.IsTestHardWare) {
			HardWareTest.Instance.JiaMiJiaoYanFailed();
		}
	}

	void OnEndJiaoYanIO(JIAOYANENUM val)
	{
		IsJiaoYanHid = false;
		if (IsInvoking("CloseJiaoYanIO")) {
			CancelInvoke("CloseJiaoYanIO");
		}

		switch (val) {
		case JIAOYANENUM.FAILED:
			JiaoYanFailedCount++;
			break;

		case JIAOYANENUM.SUCCEED:
			JiaoYanSucceedCount++;
			JiaoYanFailedCount = 0;
			if (HardWareTest.IsTestHardWare) {
				HardWareTest.Instance.JiaMiJiaoYanSucceed();
			}
			break;
		}
		JiaoYanState = val;
		//Debug.Log("*****JiaoYanState "+JiaoYanState);

		if (JiaoYanFailedCount >= JiaoYanFailedMax || IsJiOuJiaoYanFailed) {
			//JiaoYanFailed
			if (IsJiOuJiaoYanFailed) {
				//JiOuJiaoYanFailed
				//Debug.Log("JOJYSB...");
			}
			else {
				//JiaMiXinPianJiaoYanFailed
				//Debug.Log("JMXPJYSB...");
				IsJiaMiJiaoYanFailed = true;
			}
		}
	}
	public static bool IsJiaMiJiaoYanFailed;
	
	enum JIAOYANENUM
	{
		NULL,
		SUCCEED,
		FAILED,
	}
	static JIAOYANENUM JiaoYanState = JIAOYANENUM.NULL;
	static byte JiaoYanFailedMax = 0x03;
	static byte JiaoYanSucceedCount;
	static byte JiaoYanFailedCount;
	static byte[] JiaoYanDt = new byte[4];
	static byte[] JiaoYanMiMa = new byte[4];
	static byte[] JiaoYanMiMaRand = new byte[4];
	
	//#define First_pin			 	0xe5
	//#define Second_pin		 	0x5d
	//#define Third_pin		 		0x8c
	void InitJiaoYanMiMa()
	{
		JiaoYanMiMa[1] = 0xe5; //0xff;
		JiaoYanMiMa[2] = 0x5d; //0xff;
		JiaoYanMiMa[3] = 0x8c; //0xff;
		JiaoYanMiMa[0] = 0x00;
		for (int i = 1; i < 4; i++) {
			JiaoYanMiMa[0] ^= JiaoYanMiMa[i];
		}
	}

	void RandomJiaoYanMiMaVal()
	{
		for (int i = 0; i < 4; i++) {
			JiaoYanMiMaRand[i] = (byte)UnityEngine.Random.Range(0x00, (JiaoYanMiMa[i] - 1));
		}

		byte TmpVal = 0x00;
		for (int i = 1; i < 4; i++) {
			TmpVal ^= JiaoYanMiMaRand[i];
		}

		if (TmpVal == JiaoYanMiMaRand[0]) {
			JiaoYanMiMaRand[0] = JiaoYanMiMaRand[0] == 0x00 ?
				(byte)UnityEngine.Random.Range(0x01, 0xff) : (byte)(JiaoYanMiMaRand[0] + UnityEngine.Random.Range(0x01, 0xff));
		}
	}

	public static bool IsJiaoYanHid;
	public static int CountFXZD;
	public static int CountQNZD;
	public void OpenFangXiangPanZhenDong()
	{
		if (IsPlayFangXiangPanZhenDong) {
			return;
		}
		IsPlayFangXiangPanZhenDong = true;
		CountFXZD++;
		//Debug.Log("OpenFangXiangPanZhenDong -> CountFXZD "+CountFXZD+", CountQNZD "+CountQNZD);
		StartCoroutine(PlayFangXiangPanZhenDong());
	}

	public static bool IsSlowLoopCom = false;
	IEnumerator PlayFangXiangPanZhenDong()
	{
		int count = UnityEngine.Random.Range(1, 4);
		//count = 1; //test
		do {
			IsZhenDongFangXiangPan = !IsZhenDongFangXiangPan;
			count--;
			yield return new WaitForSeconds(0.05f);
		} while (count > -1);
		IsZhenDongFangXiangPan = false;
		//IsZhenDongFangXiangPan = true; //test
		IsPlayFangXiangPanZhenDong = false;
	}

	byte JiOuJiaoYanCount;
	byte JiOuJiaoYanMax = 5;
	public static bool IsJiOuJiaoYanFailed;
	public void GetMessage()
	{
        if (CheckGetMsgInfoIsError(MyCOMDevice.ComThreadClass.ReadByteMsg))
        {
            return;
        }

		//if (IsJiOuJiaoYanFailed) {
		//	return;
		//}

		//if ((MyCOMDevice.ComThreadClass.ReadByteMsg[22]&0x01) == 0x01) {
		//	JiOuJiaoYanCount++;
		//	if (JiOuJiaoYanCount >= JiOuJiaoYanMax && !IsJiOuJiaoYanFailed) {
		//		IsJiOuJiaoYanFailed = true;
		//		//JiOuJiaoYanFailed
		//	}
		//}
		//IsJiOuJiaoYanFailed = true; //test

		//byte tmpVal = 0x00;
		//string testA = "";
		//for (int i = 2; i < (MyCOMDevice.ComThreadClass.BufLenRead - 4); i++) {
		//	if (i == 8 || i == 21) {
		//		continue;
		//	}
		//	testA += MyCOMDevice.ComThreadClass.ReadByteMsg[i].ToString("X2") + " ";
		//	tmpVal ^= MyCOMDevice.ComThreadClass.ReadByteMsg[i];
		//}
		//tmpVal ^= EndRead_1;
		//tmpVal ^= EndRead_2;
		//testA += EndRead_1 + " ";
		//testA += EndRead_2 + " ";

		//if (tmpVal != MyCOMDevice.ComThreadClass.ReadByteMsg[21]) {
		//	if (MyCOMDevice.ComThreadClass.IsStopComTX) {
		//		return;
		//	}
		//	MyCOMDevice.ComThreadClass.IsStopComTX = true;
//			ScreenLog.Log("testA: "+testA);
//			ScreenLog.LogError("tmpVal: "+tmpVal.ToString("X2")+", byte[21] "+MyCOMDevice.ComThreadClass.ReadByteMsg[21].ToString("X2"));
//			ScreenLog.Log("byte21 was wrong!");
		//	return;
		//}

		//if (IsJiaoYanHid) {
		//	tmpVal = 0x00;
//			string testStrA = MyCOMDevice.ComThreadClass.ReadByteMsg[30].ToString("X2") + " ";
//			string testStrB = "";
//			string testStrA = "";
//			for (int i = 0; i < MyCOMDevice.ComThreadClass.ReadByteMsg.Length; i++) {
//				testStrA += MyCOMDevice.ComThreadClass.ReadByteMsg[i].ToString("X2") + " ";
//			}
//			ScreenLog.Log("readStr: "+testStrA);

//			for (int i = 0; i < JiaoYanDt.Length; i++) {
//				testStrB += JiaoYanDt[i].ToString("X2") + " ";
//			}
//			ScreenLog.Log("GameSendDt: "+testStrB);

//			string testStrC = "";
//			for (int i = 0; i < JiaoYanDt.Length; i++) {
//				testStrC += JiaoYanMiMa[i].ToString("X2") + " ";
//			}
//			ScreenLog.Log("GameSendMiMa: "+testStrC);

			//for (int i = 11; i < 14; i++) {
			//	tmpVal ^= MyCOMDevice.ComThreadClass.ReadByteMsg[i];
//				testStrA += MyCOMDevice.ComThreadClass.ReadByteMsg[i].ToString("X2") + " ";
			//}

			//if (tmpVal == MyCOMDevice.ComThreadClass.ReadByteMsg[10]) {
			//	bool isJiaoYanDtSucceed = false;
			//	tmpVal = 0x00;
			//	for (int i = 15; i < 18; i++) {
			//		tmpVal ^= MyCOMDevice.ComThreadClass.ReadByteMsg[i];
			//	}
				
				//校验2...
				//if ( tmpVal == MyCOMDevice.ComThreadClass.ReadByteMsg[14]
				//    && (JiaoYanDt[1]&0xef) == MyCOMDevice.ComThreadClass.ReadByteMsg[15]
				//    && (JiaoYanDt[2]&0xfe) == MyCOMDevice.ComThreadClass.ReadByteMsg[16]
				//    && (JiaoYanDt[3]|0x28) == MyCOMDevice.ComThreadClass.ReadByteMsg[17] ) {
				//	isJiaoYanDtSucceed = true;
				//}

				//if (isJiaoYanDtSucceed) {
					//JiaMiJiaoYanSucceed
					//OnEndJiaoYanIO(JIAOYANENUM.SUCCEED);
					//ScreenLog.Log("JMJYCG...");
				//}
//				else {
//					testStrA = "";
//					for (int i = 0; i < 46; i++) {
//						testStrA += MyCOMDevice.ComThreadClass.ReadByteMsg[i].ToString("X2") + " ";
//					}
//
//					string testStrC = "";
//					for (int i = 34; i < 38; i++) {
//						testStrB += MyCOMDevice.ComThreadClass.ReadByteMsg[i].ToString("X2") + " ";
//					}
//
//					for (int i = 0; i < 4; i++) {
//						testStrC += JiaoYanDt[i].ToString("X2") + " ";
//					}
//					ScreenLog.Log("ReadByte[0 - 45] "+testStrA);
//					ScreenLog.Log("ReadByte[34 - 37] "+testStrB);
//					ScreenLog.Log("SendByte[21 - 24] "+testStrC);
//					ScreenLog.LogError("校验数据错误!");
//				}
			//}
//			else {
//				testStrB = "byte[30] "+MyCOMDevice.ComThreadClass.ReadByteMsg[30].ToString("X2")+" "
//					+", tmpVal "+tmpVal.ToString("X2");
//				ScreenLog.Log("ReadByte[30 - 33] "+testStrA);
//				ScreenLog.Log(testStrB);
//				ScreenLog.LogError("ReadByte[30] was wrong!");
//			}
		//}

		CheckIsPlayerActivePcvr();
        //int len = MyCOMDevice.ComThreadClass.ReadByteMsg.Length;
        //uint[] readMsg = new uint[len];
        //for (int i = 0; i < len; i++)
        //{
        //    readMsg[i] = MyCOMDevice.ComThreadClass.ReadByteMsg[i];
        //}
        KeyProcess(MyCOMDevice.ComThreadClass.ReadByteMsg);
	}

	void CheckBikeDirLen()
	{
		BikeDirLenA = SteerValMin - SteerValCen + 1;
		BikeDirLenB = SteerValCen - SteerValMax + 1;
		BikeDirLenC = SteerValMax - SteerValCen + 1;
		BikeDirLenD = SteerValCen - SteerValMin + 1;
	}

	static bool IsHandleDirByKey = true;
	public static void GetPcvrSteerVal()
	{
		if (!IsHandleDirByKey) {
			if (!bIsHardWare) {
				mGetSteer = Input.GetAxis("Horizontal");
				return;
			}
		}
		else {
			if (!bIsHardWare || IsTestGame) {
				mGetSteer = Input.GetAxis("Horizontal");
				return;
			}
		}

		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}

		if (IsInitFangXiangJiaoZhun) {
			return;
		}

		uint bikeDir = SteerValCur;
		uint bikeDirLen = SteerValMax - SteerValMin + 1;
		if (SteerValMax < SteerValMin) {
			bikeDirLen = bikeDir > SteerValCen ? BikeDirLenA : BikeDirLenB;
			bikeDir = Math.Min(bikeDir, SteerValMin);
			bikeDir = Math.Max(bikeDir, SteerValMax);
		}
		else {
			bikeDirLen = bikeDir > SteerValCen ? BikeDirLenC : BikeDirLenD;
			bikeDir = Math.Max(bikeDir, SteerValMin);
			bikeDir = Math.Min(bikeDir, SteerValMax);
		}
		bikeDirLen = Math.Max(1, bikeDirLen);
		
		uint bikeDirCur = SteerValMax - bikeDir;
		float bikeDirPer = (float)bikeDirCur / bikeDirLen;
		if (SteerValMax > SteerValMin) {
			//ZhengJie FangXiangDianWeiQi
			if (bikeDir > SteerValCen) {
				bikeDirCur = bikeDir - SteerValCen;
				bikeDirPer = (float)bikeDirCur / bikeDirLen;
			}
			else {
				bikeDirCur = SteerValCen - bikeDir;
				bikeDirPer = - (float)bikeDirCur / bikeDirLen;
			}
		}
		else {
			//FanJie DianWeiQi
			if(bikeDir > SteerValCen) {
				bikeDirCur = bikeDir - SteerValCen;
				bikeDirPer = - (float)bikeDirCur / bikeDirLen;
			}
			else {
				bikeDirCur = SteerValCen - bikeDir;
				bikeDirPer = (float)bikeDirCur / bikeDirLen;
			}
		}
		mGetSteer = bikeDirPer;
		//Debug.Log("*** mGetSteer "+mGetSteer+", SteerValMax "+SteerValMax+", SteerValMin "+SteerValMin+", bikeDirCur "+bikeDirCur);
	}

	//static float TimePowerLast;
	//static float TimePowerMax = 3f;
	//static float PowerLastVal;
	static bool IsAddSpeed;
    //public static void GetPcvrPowerVal()
    //{
    //	//if (!bIsHardWare) {
    //	if (!bIsHardWare || IsTestGame) {
    //		float valVer = Input.GetAxis("Vertical");
    //		float powerTmp = 0f;
    //		if (valVer > 0f) {
    //			if (!IsAddSpeed) {
    //				IsAddSpeed = true;
    //				TimePowerLast = Time.realtimeSinceStartup;
    //			}

    //			if (Time.realtimeSinceStartup - TimePowerLast < TimePowerMax) {
    //				powerTmp = (Time.realtimeSinceStartup - TimePowerLast) / TimePowerMax;
    //			}
    //			else {
    //				powerTmp = 1f;
    //			}
    //		}
    //		else {
    //			if (IsAddSpeed) {
    //				IsAddSpeed = false;
    //				PowerLastVal = mGetPower;
    //				TimePowerLast = Time.realtimeSinceStartup;
    //			}

    //			if (Time.realtimeSinceStartup - TimePowerLast < TimePowerMax && mGetPower > 0f) {
    //				powerTmp = (Time.realtimeSinceStartup - TimePowerLast) / TimePowerMax;
    //				powerTmp = PowerLastVal > powerTmp ? (PowerLastVal - powerTmp) : 0f;
    //			}
    //			else {
    //				powerTmp = 0f;
    //			}
    //		}
    //		powerTmp = powerTmp <= YouMemnMinVal ? 0f : powerTmp;
    //		mGetPower = powerTmp;
    //		return;
    //	}

    //	if (!MyCOMDevice.IsFindDeviceDt) {
    //		return;
    //	}

    //	if (IsInitYouMenJiaoZhun) {
    //		return;
    //	}

    //	uint bikePowerCurValTmp = 0;
    //	if (mBikePowerMin > mBikePowerMax) {
    //		bikePowerCurValTmp = Math.Min(BikePowerCur, mBikePowerMin);
    //		bikePowerCurValTmp = Math.Max(bikePowerCurValTmp, mBikePowerMax);
    //	}
    //	else {
    //		bikePowerCurValTmp = Math.Max(BikePowerCur, mBikePowerMin);
    //		bikePowerCurValTmp = Math.Min(bikePowerCurValTmp, mBikePowerMax);
    //	}

    //	uint bikePowerDis = mBikePowerMin > mBikePowerMax ? (mBikePowerMin - bikePowerCurValTmp) : (bikePowerCurValTmp - mBikePowerMin);
    //	float valThrottleTmp = (float)bikePowerDis / BikePowerLen;
    //	valThrottleTmp = valThrottleTmp <= YouMemnMinVal ? 0f : valThrottleTmp;
    //	valThrottleTmp = valThrottleTmp > 1f ? 1f : valThrottleTmp;
    //	mGetPower = valThrottleTmp;

    //		if (IsTestGame) {
    //			mGetPower = 1f; //test
    //		}
    //}
    //public static float YouMemnMinVal = 0.1f;

    //public static void GetPcvrShaCheVal()
    //{
    //	if (!bIsHardWare) {
    //		return;
    //	}

    //	if (!MyCOMDevice.IsFindDeviceDt) {
    //		return;
    //	}

    //	if (IsInitShaCheJiaoZhun) {
    //		return;
    //	}

    //	uint bikeShaCheCurValTmp = 0;
    //	if (mBikeShaCheMin > mBikeShaCheMax) {
    //		bikeShaCheCurValTmp = Math.Min(BikeShaCheCur, mBikeShaCheMin);
    //		bikeShaCheCurValTmp = Math.Max(bikeShaCheCurValTmp, mBikeShaCheMax);
    //	}
    //	else {
    //		bikeShaCheCurValTmp = Math.Max(BikeShaCheCur, mBikeShaCheMin);
    //		bikeShaCheCurValTmp = Math.Min(bikeShaCheCurValTmp, mBikeShaCheMax);
    //	}

    //	uint bikeShaCheDis = mBikeShaCheMin > mBikeShaCheMax ? (mBikeShaCheMin - bikeShaCheCurValTmp) : (bikeShaCheCurValTmp - mBikeShaCheMin);
    //	float valTmp = (float)bikeShaCheDis / BikeShaCheLen;
    //	valTmp = valTmp <= 0.3f ? 0f : 1f;
    //	if (IsTestGame) {
    //		return; //test
    //	}

    //		if (!IsActiveSheCheEvent && valTmp > 0.3f) {
    //			IsActiveSheCheEvent = true;
    //			InputEventCtrl.GetInstance().ClickShaCheBt( ButtonState.DOWN );
    //		}
    //		else if (IsActiveSheCheEvent && valTmp < 0.3f){
    //			IsActiveSheCheEvent = false;
    //			InputEventCtrl.GetInstance().ClickShaCheBt( ButtonState.UP );
    //		}
    //}

    int[] PlayerCoinHidArray = new int[4];
    [HideInInspector]
    public int[] PlayerCoinArray = new int[4];
    public void SubPlayerCoin(int subNum, PlayerCoinEnum indexPlayer)
	{
        int indexVal = (int)indexPlayer;
        if (PlayerCoinArray[indexVal] > subNum)
        {
            PlayerCoinArray[indexVal] -= subNum;
        }
	}
	
	//public void InitYouMenJiaoZhun()
	//{
	//	if (IsInitYouMenJiaoZhun) {
	//		return;
	//	}
	//	//ScreenLog.Log("pcvr -> InitYouMenJiaoZhun...");
	//	mBikePowerMin = 999999;
	//	mBikePowerMax = 0;
		
	//	IsJiaoZhunFireBt = false;
	//	IsInitYouMenJiaoZhun = true;
	//}
	
	//void ResetYouMenJiaoZhun()
	//{
	//	if (!IsInitYouMenJiaoZhun) {
	//		return;
	//	}
	//	//ScreenLog.Log("pcvr -> ResetYouMenJiaoZhun...");
	//	IsJiaoZhunFireBt = false;
	//	IsInitYouMenJiaoZhun = false;
	//	bIsJiaoYanBikeValue = false;
		
	//	uint TmpVal = 0;
	//	if (IsFanZhuangYouMen) {
	//		TmpVal = mBikePowerMax;
	//		mBikePowerMax = mBikePowerMin;
	//		mBikePowerMin = TmpVal;
	//		BikePowerLen = mBikePowerMin - mBikePowerMax + 1;
	//		//ScreenLog.Log("YouMenFanZhuang -> mBikePowerMax = " + mBikePowerMax + ", mBikePowerMin = " + mBikePowerMin);
	//	}
	//	else {
	//		BikePowerLen = mBikePowerMax - mBikePowerMin + 1;
	//		//ScreenLog.Log("YouMenZhengZhuang -> mBikePowerMax = " + mBikePowerMax + ", mBikePowerMin = " + mBikePowerMin);
	//	}
	//	BikePowerLen = Math.Max(1, BikePowerLen);

	//	PlayerPrefs.SetInt("mBikePowerMax", (int)mBikePowerMax);
	//	PlayerPrefs.SetInt("mBikePowerMin", (int)mBikePowerMin);
	//}

	//public void InitShaCheJiaoZhun()
	//{
	//	if (IsInitShaCheJiaoZhun) {
	//		return;
	//	}
	//	mBikeShaCheMin = 999999;
	//	mBikeShaCheMax = 0;
	//	IsJiaoZhunFireBt = false;
	//	IsInitShaCheJiaoZhun = true;
	//}

	//void ResetShaCheJiaoZhun()
	//{
	//	if (!IsInitShaCheJiaoZhun) {
	//		return;
	//	}
	//	IsJiaoZhunFireBt = false;
	//	IsInitShaCheJiaoZhun = false;
	//	bIsJiaoYanBikeValue = false;
		
	//	uint TmpVal = 0;
	//	if (IsFanZhuangShaChe) {
	//		TmpVal = mBikeShaCheMax;
	//		mBikeShaCheMax = mBikeShaCheMin;
	//		mBikeShaCheMin = TmpVal;
	//		BikeShaCheLen = mBikeShaCheMin - mBikeShaCheMax + 1;
	//	}
	//	else {
	//		BikeShaCheLen = mBikeShaCheMax - mBikeShaCheMin + 1;
	//	}
	//	BikeShaCheLen = Math.Max(1, BikeShaCheLen);

	//	PlayerPrefs.SetInt("mBikeShaCheMax", (int)mBikeShaCheMax);
	//	PlayerPrefs.SetInt("mBikeShaCheMin", (int)mBikeShaCheMin);
	//}

	public void InitFangXiangJiaoZhun()
	{
		if (IsInitFangXiangJiaoZhun) {
			return;
		}
		//ScreenLog.Log("pcvr -> InitFangXiangJiaoZhun...");
		//FangXiangInfo
		SteerValMin = 999999;
		SteerValCen = 1765;
		SteerValMax = 0;
		
		//IsJiaoZhunFireBt = false;
		FangXiangJiaoZhunCount = 0;
		IsInitFangXiangJiaoZhun = true;
		bIsJiaoYanBikeValue = true;
	}
	
	void ResetFangXiangJiaoZhun()
	{
		if (!IsInitFangXiangJiaoZhun) {
			return;
		}
		//ScreenLog.Log("pcvr -> ResetFangXiangJiaoZhun...");
		//IsJiaoZhunFireBt = false;
		FangXiangJiaoZhunCount = 0;
		IsInitFangXiangJiaoZhun = false;
		
		uint TmpVal = 0;
		if (IsFanZhuangFangXiang) {
			TmpVal = SteerValMax;
			SteerValMax = SteerValMin;
			SteerValMin = TmpVal;
			//ScreenLog.Log("CheTouFangXiangFanZhuan -> SteerValMin " + SteerValMin + ", SteerValMax " +SteerValMax);
		}
		else {
			//ScreenLog.Log("CheTouFangXiangZhengZhuan -> SteerValMin " + SteerValMin + ", SteerValMax " +SteerValMax);
		}
		CheckBikeDirLen();
		PlayerPrefs.SetInt("mBikeDirMin", (int)SteerValMin);
		PlayerPrefs.SetInt("mBikeDirCen", (int)SteerValCen);
		PlayerPrefs.SetInt("mBikeDirMax", (int)SteerValMax);
	}

	//void ShaCheJiaoZhun()
	//{
	//	if (!IsInitShaCheJiaoZhun) {
	//		return;
	//	}
		
	//	if (BikeShaCheCur < mBikeShaCheMin) {
	//		mBikeShaCheMin = BikeShaCheCur;
	//		PlayerPrefs.SetInt("mBikeShaCheMin", (int)mBikeShaCheMin);
	//	}
		
	//	if (BikeShaCheCur > mBikeShaCheMax) {
	//		mBikeShaCheMax = BikeShaCheCur;
	//		PlayerPrefs.SetInt("mBikeShaCheMax", (int)mBikeShaCheMax);
	//	}
		
	//	if (bPlayerStartKeyDown && !IsJiaoZhunFireBt) {
	//		IsJiaoZhunFireBt = true;
	//		uint dVal_0 = BikeShaCheCur - mBikeShaCheMin;
	//		uint dVal_1 = mBikeShaCheMax - BikeShaCheCur;
	//		if (dVal_0 > dVal_1) {
	//			IsFanZhuangShaChe = false;
	//		}
	//		else if (dVal_0 < dVal_1) {
	//			IsFanZhuangShaChe = true;
	//		}
	//		ResetShaCheJiaoZhun();
	//	}
	//	else if(!bPlayerStartKeyDown && IsJiaoZhunFireBt) {
	//		IsJiaoZhunFireBt = false;
	//	}
	//}

	//void YouMenJiaoZhun()
	//{
	//	if (!IsInitYouMenJiaoZhun) {
	//		return;
	//	}

	//	if (BikePowerCur < mBikePowerMin) {
	//		mBikePowerMin = BikePowerCur;
	//		PlayerPrefs.SetInt("mBikePowerMin", (int)mBikePowerMin);
	//	}
		
	//	if (BikePowerCur > mBikePowerMax) {
	//		mBikePowerMax = BikePowerCur;
	//		PlayerPrefs.SetInt("mBikePowerMax", (int)mBikePowerMax);
	//	}
		
	//	if (bPlayerStartKeyDown && !IsJiaoZhunFireBt) {
	//		IsJiaoZhunFireBt = true;
	//		uint dVal_0 = BikePowerCur - mBikePowerMin;
	//		uint dVal_1 = mBikePowerMax - BikePowerCur;
	//		if (dVal_0 > dVal_1) {
	//			//YouMenZhengZhuang
	//			IsFanZhuangYouMen = false;
	//		}
	//		else if (dVal_0 < dVal_1) {
	//			//YouMenFanZhuang
	//			IsFanZhuangYouMen = true;
	//		}
	//		ResetYouMenJiaoZhun();
	//		//InitShaCheJiaoZhun();
	//		IsJiaoZhunFireBt = true;
	//	}
	//	else if(!bPlayerStartKeyDown && IsJiaoZhunFireBt) {
	//		IsJiaoZhunFireBt = false;
	//	}
	//}

	void FangXiangJiaoZhun()
	{
		if (!IsInitFangXiangJiaoZhun) {
			return;
		}
		
		//Record FangXiangInfo
		if (SteerValCur < SteerValMin) {
			SteerValMin = SteerValCur;
			PlayerPrefs.SetInt("mBikeDirMin", (int)SteerValMin);
		}
		
		if (SteerValCur > SteerValMax) {
			SteerValMax = SteerValCur;
			PlayerPrefs.SetInt("mBikeDirMax", (int)SteerValMax);
		}
		
		//if (bPlayerStartKeyDown && !IsJiaoZhunFireBt) {
			//IsJiaoZhunFireBt = true;
		//	FangXiangJiaoZhunCount++;
		//	switch (FangXiangJiaoZhunCount) {
		//	case 1:
		//		//CheTouZuoZhuan
		//		uint dVal_0 = SteerValCur - SteerValMin;
		//		uint dVal_1 = SteerValMax - SteerValCur;
		//		if (dVal_0 < dVal_1) {
		//			IsFanZhuangFangXiang = false;
		//		}
		//		else if (dVal_0 > dVal_1) {
		//			IsFanZhuangFangXiang = true;
		//		}
		//		break;
				
		//	case 2:
		//		//CheTouZhuanDaoZhongJian
		//		SteerValCen = SteerValCur;
		//		break;
				
		//	case 3:
		//		//CheTouYouZhuan
		//		ResetFangXiangJiaoZhun();
		//		//InitYouMenJiaoZhun();
		//		//IsJiaoZhunFireBt = true;
		//		break;
		//	}
		//}
		//else if(!bPlayerStartKeyDown && IsJiaoZhunFireBt) {
		//	IsJiaoZhunFireBt = false;
		//}
	}

	//public static uint BikeBeiYongPowerCurPcvr;
	void KeyProcess(byte []buffer)
	{
		if (!MyCOMDevice.IsFindDeviceDt) {
			return;
		}

		if (buffer[0] != ReadHead_1 || buffer[1] != ReadHead_2) {
			return;
		}

        //SteerValCur = (((uint)buffer[6]&0x0f) << 8) + buffer[7]; //fangXiang
        //bool isTest = false;
        //if (!isTest) {
        //	BikePowerCur = (((uint)buffer[2]&0x0f) << 8) + buffer[3]; //youMen
        //	BikePowerCurPcvr = BikePowerCur;

        //	BikeShaCheCur = (((uint)buffer[4]&0x0f) << 8) + buffer[5]; //shaChe
        //	ShaCheCurPcvr = BikeShaCheCur;
        //}
        //else {
        //	BikePowerCur = SteerValCur; //test
        //	BikeShaCheCur = SteerValCur; //test
        //}

        //if (HardWareTest.IsTestHardWare) {
        //	uint tmpBYYouMen = (((uint)buffer[2]&0x0f) << 8) + buffer[3]; //youMen
        //	BikeBeiYongPowerCurPcvr = tmpBYYouMen;
        //}

        //		if (!IsInitYouMenJiaoZhun) {
        //			float dPower = BikePowerOld > BikePowerCur ? BikePowerOld - BikePowerCur : BikePowerCur - BikePowerOld;
        //			if (mBikePowerMax > mBikePowerMin) {
        //				if (dPower / (mBikePowerMax - mBikePowerMin) > 0.3f) {
        //					BikePowerCur = mBikePowerMin;
        //				}
        //			}
        //			else {
        //				if (dPower / (mBikePowerMin - mBikePowerMax) > 0.3f) {
        //					BikePowerCur = mBikePowerMax;
        //				}
        //			}
        //			BikePowerOld = BikePowerCur;
        //		}

        //game coinInfo
        PlayerCoinHidArray[0] = buffer[18] & 0x0f;
        PlayerCoinHidArray[1] = (buffer[18] & 0xf0) >> 4;
        PlayerCoinHidArray[2] = buffer[19] & 0x0f;
        PlayerCoinHidArray[3] = (buffer[19] & 0xf0) >> 4;
        CheckPlayerCoinInfo(PlayerCoinEnum.player01);
        CheckPlayerCoinInfo(PlayerCoinEnum.player02);
        CheckPlayerCoinInfo(PlayerCoinEnum.player03);
        CheckPlayerCoinInfo(PlayerCoinEnum.player04);

		if (bIsJiaoYanBikeValue) {
			FangXiangJiaoZhun();
			//YouMenJiaoZhun();
			//ShaCheJiaoZhun();
		}

        CaiPiaoPrintState caiPiaoPrintSt01 = (CaiPiaoPrintState)buffer[44];
        //CaiPiaoPrintState caiPiaoPrintSt02 = (CaiPiaoPrintState)buffer[44];
        OnReceiveCaiPiaoJiPrintState(caiPiaoPrintSt01, CaiPiaoJi.Num01);
        //CheckCaiPiaoJiPrintState(caiPiaoPrintSt02, CaiPiaoJi.Num02);

        //按键1 - 动感控制开关
        //if (buffer[21] == 0x00 || buffer[21] == 0xff)
        //{
        //}
        //else
        //{
        //    if (buffer[20] == 0x00 || buffer[20] == 0xff)
        //    {
        //    }
        //    else
        //    {
        //        if ((buffer[21] & 0x10) == 0x10)
        //        {
        //            if (IsCloseDongGanBtDown && (buffer[20] & 0x04) == 0x04)
        //            {
        //                //按键弹起.
        //                //ScreenLog.Log("dongGanBt up!");
        //                IsCloseDongGanBtDown = false;
        //                //InputEventCtrl.GetInstance().ClickCloseDongGanBt(ButtonState.UP);
        //            }
        //            else if (!IsCloseDongGanBtDown && (buffer[20] & 0x04) == 0x00)
        //            {
        //                //按键按下.
        //                //ScreenLog.Log("dongGanBt down!");
        //                IsCloseDongGanBtDown = true;
        //                //InputEventCtrl.GetInstance().ClickCloseDongGanBt(ButtonState.DOWN);
        //            }
        //        }
        //        else if ((buffer[21] & 0x40) == 0x40)
        //        {
        //            if (IsCloseDongGanBtDown && (buffer[20] & 0x10) == 0x10)
        //            {
        //                //按键弹起.
        //                //ScreenLog.Log("dongGanBt up!");
        //                IsCloseDongGanBtDown = false;
        //                //InputEventCtrl.GetInstance().ClickCloseDongGanBt(ButtonState.UP);
        //            }
        //            else if (!IsCloseDongGanBtDown && (buffer[20] & 0x10) == 0x00)
        //            {
        //                //按键按下.
        //                //ScreenLog.Log("dongGanBt down!");
        //                IsCloseDongGanBtDown = true;
        //                //InputEventCtrl.GetInstance().ClickCloseDongGanBt(ButtonState.DOWN);
        //            }
        //        }
        //    }
        //}

        //		if ( !IsCloseDongGanBtDown && 0x02 == (buffer[9]&0x02) ) {
        ////			ScreenLog.Log("game DongGanBt down!");
        //			IsCloseDongGanBtDown = true;
        //			InputEventCtrl.GetInstance().ClickCloseDongGanBt( ButtonState.DOWN );
        //		}
        //		else if ( IsCloseDongGanBtDown && 0x00 == (buffer[9]&0x02) ) {
        ////			ScreenLog.Log("game DongGanBt up!");
        //			IsCloseDongGanBtDown = false;
        //			InputEventCtrl.GetInstance().ClickCloseDongGanBt( ButtonState.UP );
        //		}

        //if ( !bPlayerStartKeyDown && 0x01 == (buffer[28]&0x01) ) { //test
        //		if ( !bPlayerStartKeyDown && 0x01 == (buffer[9]&0x01) ) {
        ////			ScreenLog.Log("game startBt down!");
        //			bPlayerStartKeyDown = true;
        //			InputEventCtrl.GetInstance().ClickStartBtOne( ButtonState.DOWN );
        //		}
        //		//else if ( bPlayerStartKeyDown && 0x00 == (buffer[28]&0x01) ) { //test
        //		else if ( bPlayerStartKeyDown && 0x00 == (buffer[9]&0x01) ) {
        ////			ScreenLog.Log("game startBt up!");
        //			bPlayerStartKeyDown = false;
        //			InputEventCtrl.GetInstance().ClickStartBtOne( ButtonState.UP );
        //		}

        //按键2 - 设置确定按键.
        //if (buffer[22] == 0x00 || buffer[22] == 0xff)
        //{
        //}
        //else
        //{
        //    if (buffer[24] == 0x00 || buffer[24] == 0xff)
        //    {
        //    }
        //    else
        //    {
        //        if ((buffer[22] & 0x10) == 0x10)
        //        {
        //            if (bSetEnterKeyDown && (buffer[24] & 0x20) == 0x20)
        //            {
        //                //按键弹起.
        //                //ScreenLog.Log("setEnterBt up!");
        //                bSetEnterKeyDown = false;
        //                //InputEventCtrl.GetInstance().ClickSetEnterBt(ButtonState.UP);
        //            }
        //            else if (!bSetEnterKeyDown && (buffer[24] & 0x20) == 0x00)
        //            {
        //                //按键按下.
        //                //ScreenLog.Log("setEnterBt down!");
        //                bSetEnterKeyDown = true;
        //                //InputEventCtrl.GetInstance().ClickSetEnterBt(ButtonState.DOWN);
        //            }
        //        }
        //        else if ((buffer[22] & 0x40) == 0x40)
        //        {
        //            if (bSetEnterKeyDown && (buffer[24] & 0x80) == 0x80)
        //            {
        //                //按键弹起.
        //                //ScreenLog.Log("setEnterBt up!");
        //                bSetEnterKeyDown = false;
        //                //InputEventCtrl.GetInstance().ClickSetEnterBt(ButtonState.UP);
        //            }
        //            else if (!bSetEnterKeyDown && (buffer[24] & 0x80) == 0x00)
        //            {
        //                //按键按下.
        //                //ScreenLog.Log("setEnterBt down!");
        //                bSetEnterKeyDown = true;
        //                //InputEventCtrl.GetInstance().ClickSetEnterBt(ButtonState.DOWN);
        //            }
        //        }
        //    }
        //}

        //        if ( !bSetEnterKeyDown && 0x10 == (buffer[9]&0x10) ) {
        //			bSetEnterKeyDown = true;
        ////			ScreenLog.Log("game setEnterBt down!");
        //			InputEventCtrl.GetInstance().ClickSetEnterBt( ButtonState.DOWN );
        //		}
        //		else if ( bSetEnterKeyDown && 0x00 == (buffer[9]&0x10) ) {
        //			bSetEnterKeyDown = false;
        ////			ScreenLog.Log("game setEnterBt up!");
        //			InputEventCtrl.GetInstance().ClickSetEnterBt( ButtonState.UP );
        //		}

        //按键8 - 设置移动按键.
        //if (buffer[25] == 0x00 || buffer[25] == 0xff)
        //{
        //}
        //else
        //{
        //    if (buffer[27] == 0x00 || buffer[27] == 0xff)
        //    {
        //    }
        //    else
        //    {
        //        if ((buffer[25] & 0x10) == 0x10)
        //        {
        //            if (bSetMoveKeyDown && (buffer[27] & 0x02) == 0x02)
        //            {
        //                //按键弹起.
        //                //ScreenLog.Log("setMoveBt up!");
        //                bSetMoveKeyDown = false;
        //                //InputEventCtrl.GetInstance().ClickSetMoveBt(ButtonState.UP);
        //            }
        //            else if (!bSetMoveKeyDown && (buffer[27] & 0x02) == 0x00)
        //            {
        //                //按键按下.
        //                //ScreenLog.Log("setMoveBt down!");
        //                bSetMoveKeyDown = true;
        //                //InputEventCtrl.GetInstance().ClickSetMoveBt(ButtonState.DOWN);
        //            }
        //        }
        //        else if ((buffer[25] & 0x40) == 0x40)
        //        {
        //            if (bSetMoveKeyDown && (buffer[27] & 0x10) == 0x10)
        //            {
        //                //按键弹起.
        //                //ScreenLog.Log("setMoveBt up!");
        //                bSetMoveKeyDown = false;
        //                //InputEventCtrl.GetInstance().ClickSetMoveBt(ButtonState.UP);
        //            }
        //            else if (!bSetMoveKeyDown && (buffer[27] & 0x10) == 0x00)
        //            {
        //                //按键按下.
        //                //ScreenLog.Log("setMoveBt down!");
        //                bSetMoveKeyDown = true;
        //                //InputEventCtrl.GetInstance().ClickSetMoveBt(ButtonState.DOWN);
        //            }
        //        }
        //    }
        //}

        //        if ( !bSetMoveKeyDown && 0x20 == (buffer[9]&0x20) ) {
        //			bSetMoveKeyDown = true;
        ////			ScreenLog.Log("game setMoveBt down!");
        //			InputEventCtrl.GetInstance().ClickSetMoveBt( ButtonState.DOWN );
        //		}
        //		else if( bSetMoveKeyDown && 0x00 == (buffer[9]&0x20) ) {
        //			bSetMoveKeyDown = false;
        ////			ScreenLog.Log("game setMoveBt up!");
        //			InputEventCtrl.GetInstance().ClickSetMoveBt( ButtonState.UP );
        //		}

        //		if ( !IsClickLaBaBt && 0x04 == (buffer[9]&0x04) ) {
        //			IsClickLaBaBt = true;
        ////			ScreenLog.Log("game LaBaBt down!");
        //			InputEventCtrl.GetInstance().ClickLaBaBt( ButtonState.DOWN );
        //		}
        //		else if( IsClickLaBaBt && 0x00 == (buffer[9]&0x04) ) {
        //			IsClickLaBaBt = false;
        ////			ScreenLog.Log("game LaBaBt up!");
        //			InputEventCtrl.GetInstance().ClickLaBaBt( ButtonState.UP );
        //		}
    }

	public static bool IsPlayerActivePcvr = true;
	public static float TimeLastActivePcvr;
	void CheckIsPlayerActivePcvr()
	{
		if (Application.loadedLevel >= 1) {
			return;
		}

		if (!IsPlayerActivePcvr) {
			return;
		}
		
		if (Time.realtimeSinceStartup - TimeLastActivePcvr > 60f) {
			IsPlayerActivePcvr = false;
		}
	}
	
	public static void SetIsPlayerActivePcvr()
	{
		if (!bIsHardWare) {
			return;
		}
		IsPlayerActivePcvr = true;
		TimeLastActivePcvr = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 设置彩票机打印命令.
    /// </summary>
    public void SetCaiPiaoPrintCmd(CaiPiaoPrintCmd printCmd, CaiPiaoJi indexCaiPiaoJi, int caiPiaoCount = 1)
    {
        Debug.Log("SetCaiPiaoPrintState -> printCmd " + printCmd + ", indexCaiPiaoJi " + indexCaiPiaoJi + ", caiPiaoCount " + caiPiaoCount);
        CaiPiaoPrintCmdVal[(int)indexCaiPiaoJi] = printCmd;
        if (printCmd == CaiPiaoPrintCmd.QuanPiaoPrint || printCmd == CaiPiaoPrintCmd.BanPiaoPrint)
        {
            CaiPiaoCountPrint[(int)indexCaiPiaoJi] = caiPiaoCount;
        }
    }

    void OnReceiveCaiPiaoJiPrintState(CaiPiaoPrintState printSt, CaiPiaoJi indexCaiPiaoJi)
    {
        switch (printSt)
        {
            case CaiPiaoPrintState.WuXiao:
                {
                    if (CaiPiaoCountPrint[(int)indexCaiPiaoJi] > 0)
                    {
                        SetCaiPiaoPrintCmd(CaiPiaoPrintCmd.QuanPiaoPrint, indexCaiPiaoJi, CaiPiaoCountPrint[(int)indexCaiPiaoJi]);
                    }
                    break;
                }
            case CaiPiaoPrintState.Succeed:
                {
                    Debug.Log("CaiPiaoJi_" + indexCaiPiaoJi + " -> print succeed!");
                    SetCaiPiaoPrintCmd(CaiPiaoPrintCmd.StopPrint, indexCaiPiaoJi);
                    if (CaiPiaoJiPrintStArray[(int)indexCaiPiaoJi] != CaiPiaoPrintState.Succeed)
                    {
                        CaiPiaoCountPrint[(int)indexCaiPiaoJi] -= 1;
                    }
                    break;
                }
            case CaiPiaoPrintState.Failed:
                {
                    Debug.Log("CaiPiaoJi_" + indexCaiPiaoJi + " -> print failed!");
                    SetCaiPiaoPrintCmd(CaiPiaoPrintCmd.StopPrint, indexCaiPiaoJi);
                    break;
                }
        }
        CaiPiaoJiPrintStArray[(int)indexCaiPiaoJi] = printSt;
    }

    public enum PlayerCoinEnum
    {
        player01 = 0,
        player02 = 1,
        player03 = 2,
        player04 = 3,
    }

    /// <summary>
    /// 检测玩家的投币信息.
    /// </summary>
    void CheckPlayerCoinInfo(PlayerCoinEnum indexPlayer)
    {
        int indexVal = (int)indexPlayer;
        if (PlayerCoinHidArray[indexVal] > 0)
        {
            if (!IsCleanHidCoinArray[indexVal])
            {
                IsCleanHidCoinArray[indexVal] = true;
                PlayerCoinArray[indexVal] += PlayerCoinHidArray[indexVal];
                Debug.Log(indexPlayer + " insert coin, coinNum -> " + PlayerCoinArray[indexVal]);
            }
        }
        else
        {
            IsCleanHidCoinArray[indexVal] = false;
        }
    }

    /// <summary>
    /// 检测ADKey是否错误.
    /// </summary>
    public bool CheckADKeyIsError(byte buffer)
    {
        bool isError = false;
        //AD信息无效标记 2、5、8位依次位010
        if ((buffer & 0x02) == 0x02
            || (buffer & 0x10) != 0x10
            || (buffer & 0x80) == 0x80)
        {
            //Debug.LogWarning("UpdateDianWeiQiDt -> ADKey was wrong! key " + buffer.ToString("X2"));
            isError = true;
        }
        return isError;
    }

    bool IsCanCheckReadMsg;
    /// <summary>
    /// 检测获取的IO信息是否错误.
    /// </summary>
    /// <returns></returns>
    public bool CheckGetMsgInfoIsError(byte[] buffer)
    {
        if (!MyCOMDevice.IsFindDeviceDt)
        {
            return true;
        }

        if (!MyCOMDevice.ComThreadClass.IsReadComMsg)
        {
            return true;
        }

        if (MyCOMDevice.ComThreadClass.IsReadMsgComTimeOut)
        {
            return true;
        }

        if (MyCOMDevice.ComThreadClass.ReadByteMsg.Length < MyCOMDevice.ComThreadClass.BufLenRead)
        {
            Debug.LogWarning("ReadBufLen was wrong! len is " + MyCOMDevice.ComThreadClass.ReadByteMsg.Length);
            return true;
        }

        if (MyCOMDevice.ComThreadClass.ReadCount < 3 && !IsCanCheckReadMsg)
        {
            //抛掉前几包无效数据.
            return true;
        }
        IsCanCheckReadMsg = true;

        bool isErrorMsg = false;
        if (buffer[0] != ReadHead_1)
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> readHead01 was wrong! head01 " + buffer[0].ToString("X2"));
        }

        if (buffer[1] != ReadHead_2)
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> readHead02 was wrong! head02 " + buffer[1].ToString("X2"));
        }

        if (buffer[58] != EndRead_1)
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> readEnd01 was wrong! end01 " + buffer[58].ToString("X2"));
        }

        if (buffer[59] != EndRead_2)
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> readEnd02 was wrong! end02 " + buffer[59].ToString("X2"));
        }

        //校验位1 位号6~55的疑惑校验值、初始校验异或值为0x38，不包含53自身
        byte jiaoYanVal = 0x38;
        for (int i = 6; i <= 51; i++)
        {
            if (i != 53)
            {
                jiaoYanVal ^= buffer[i];
            }
        }

        if (jiaoYanVal != buffer[53])
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> jiaoYanVal01 was wrong! key " + buffer[53].ToString("X2"));
        }

        //数据校验位2	数据位5~49的异或值、初始异或值为0x95，不包23自身
        jiaoYanVal = 0x95;
        for (int i = 5; i <= 49; i++)
        {
            if (i != 23)
            {
                jiaoYanVal ^= buffer[i];
            }
        }

        if (jiaoYanVal != buffer[23])
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> jiaoYanVal02 was wrong! key " + buffer[23].ToString("X2"));
        }

        //全包校验	异或初值0x36、0~59都包含, 不包55自身
        jiaoYanVal = 0x36;
        for (int i = 0; i <= 59; i++)
        {
            if (i != 55)
            {
                jiaoYanVal ^= buffer[i];
            }
        }

        if (jiaoYanVal != buffer[55])
        {
            isErrorMsg = true;
            Debug.LogWarning("CheckGetMsgInfo -> jiaoYanValQuanBao was wrong! key " + buffer[55].ToString("X2"));
        }

        if (isErrorMsg)
        {
            string readInfo = "";
            for (int i = 0; i < buffer.Length; i++)
            {
                readInfo += buffer[i].ToString("X2") + " ";
            }
            Debug.LogWarning("readMsg: " + readInfo);
        }
        return isErrorMsg;
    }

    /// <summary>
    /// 获取继电器控制命令.
    /// </summary>
    byte GetJiDianQiCmd()
    {
        byte jiDianQiCmd = 0x00;
        switch (JiDianQiCmdArray[0])
        {
            case JiDianQiCmd.Close:
                {
                    jiDianQiCmd |= 0xa0;
                    break;
                }
            case JiDianQiCmd.Open:
                {
                    jiDianQiCmd |= 0x50;
                    break;
                }
        }

        switch (JiDianQiCmdArray[1])
        {
            case JiDianQiCmd.Close:
                {
                    jiDianQiCmd |= 0x0a;
                    break;
                }
            case JiDianQiCmd.Open:
                {
                    jiDianQiCmd |= 0x05;
                    break;
                }
        }
        return jiDianQiCmd;
    }

    public bool CheckAnJianInfoIsError(byte buffer)
    {
        //键值有效位 2、3、5、7分别是1101
        if ((buffer & 0x02) != 0x02
            || (buffer & 0x04) != 0x04
            || (buffer & 0x10) == 0x10
            || (buffer & 0x40) != 0x40)
        {
            Debug.LogWarning("UpdateAnJianLbDt -> btKey was wrong! key is " + buffer.ToString("X2"));
            return true;
        }
        return false;
    }
}

public enum StartLightState
{
	Liang,
	Shan,
	Mie
}