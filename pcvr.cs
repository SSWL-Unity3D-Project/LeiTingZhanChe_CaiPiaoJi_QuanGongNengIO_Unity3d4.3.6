using UnityEngine;

public class pcvr : MonoBehaviour
{
    static public bool bIsHardWare = true;
    byte ReadHead_1 = 0x53;
    byte ReadHead_2 = 0x57;
    byte EndRead_1 = 0x0d;
    byte EndRead_2 = 0x0a;
    byte WriteHead_1 = 0x09;
    byte WriteHead_2 = 0x05;
    byte WriteEnd_1 = 0x0d;
    byte WriteEnd_2 = 0x0a;
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
        Null = -1,
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
    /// <summary>
    /// 是否清除hid币值.
    /// </summary>
    bool[] IsCleanHidCoinArray = new bool[4];
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
		}
		return Instance;
	}
    
	void Start()
	{
		InitJiaoYanMiMa();
	}

	void FixedUpdate()
	{
		if (!bIsHardWare) {
			return;
		}
		SendMessage();
		GetMessage();
	}
	
	void SendMessage()
	{
		if (!MyCOMDevice.GetInstance().IsFindDeviceDt) {
			return;
		}

		byte[] buffer = new byte[MyCOMDevice.ComThreadClass.BufLenWrite];
		for (int i = 5; i < (MyCOMDevice.ComThreadClass.BufLenWrite - 2); i++) {
			buffer[i] = (byte)UnityEngine.Random.Range(0x00, 0xff);
		}
		buffer[0] = WriteHead_1;
		buffer[1] = WriteHead_2;
		buffer[MyCOMDevice.ComThreadClass.BufLenWrite - 2] = WriteEnd_1;
		buffer[MyCOMDevice.ComThreadClass.BufLenWrite - 1] = WriteEnd_2;
		
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
        if (Random.Range(0, 100) % 2 == 0)
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

    /// <summary>
    /// 随机校验数据.
    /// </summary>
    void RandomJiaoYanDt()
	{	
		for (int i = 1; i < 4; i++) {
			JiaoYanDt[i] = (byte)Random.Range(0x00, 0x7b);
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

		if (JiaoYanFailedCount >= JiaoYanFailedMax) {
            //加密校验失败.
            //Debug.Log("JMXPJYSB...");
            IsJiaMiJiaoYanFailed = true;
        }
	}

    bool IsJiaMiJiaoYanFailed;
	enum JIAOYANENUM
	{
		NULL,
		SUCCEED,
		FAILED,
	}
	JIAOYANENUM JiaoYanState = JIAOYANENUM.NULL;
	byte JiaoYanFailedMax = 0x03;
	byte JiaoYanSucceedCount;
	byte JiaoYanFailedCount;
	byte[] JiaoYanDt = new byte[4];
	byte[] JiaoYanMiMa = new byte[4];
	byte[] JiaoYanMiMaRand = new byte[4];
	
    /// <summary>
    /// 初始化加密校验.
    /// </summary>
	void InitJiaoYanMiMa()
    {
        //#define First_pin			 	0xe5
        //#define Second_pin		 	0x5d
        //#define Third_pin		 		0x8c
        JiaoYanMiMa[1] = 0xe5; //0xff;
		JiaoYanMiMa[2] = 0x5d; //0xff;
		JiaoYanMiMa[3] = 0x8c; //0xff;
		JiaoYanMiMa[0] = 0x00;
		for (int i = 1; i < 4; i++) {
			JiaoYanMiMa[0] ^= JiaoYanMiMa[i];
		}
	}

    /// <summary>
    /// 随机校验密码.
    /// </summary>
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

    /// <summary>
    /// 是否校验hid.
    /// </summary>
	static public bool IsJiaoYanHid;

    /// <summary>
    /// 获取IO板的信息.
    /// </summary>
	void GetMessage()
	{
        if (CheckGetMsgInfoIsError(MyCOMDevice.ComThreadClass.ReadByteMsg))
        {
            return;
        }

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
        KeyProcess(MyCOMDevice.ComThreadClass.ReadByteMsg);
	}

    /// <summary>
    /// 循环检测收到IO板的信息.
    /// </summary>
	void KeyProcess(byte []buffer)
	{        
        //game coinInfo
        PlayerCoinHidArray[0] = buffer[18] & 0x0f;
        PlayerCoinHidArray[1] = (buffer[18] & 0xf0) >> 4;
        PlayerCoinHidArray[2] = buffer[19] & 0x0f;
        PlayerCoinHidArray[3] = (buffer[19] & 0xf0) >> 4;
        CheckPlayerCoinInfo(PlayerCoinEnum.player01);
        CheckPlayerCoinInfo(PlayerCoinEnum.player02);
        CheckPlayerCoinInfo(PlayerCoinEnum.player03);
        CheckPlayerCoinInfo(PlayerCoinEnum.player04);

        UpdateDianWeiQiDt(buffer);
        
        CaiPiaoPrintState caiPiaoPrintSt01 = (CaiPiaoPrintState)buffer[44];
        //CaiPiaoPrintState caiPiaoPrintSt02 = (CaiPiaoPrintState)buffer[44];
        OnReceiveCaiPiaoJiPrintState(caiPiaoPrintSt01, CaiPiaoJi.Num01);
        //CheckCaiPiaoJiPrintState(caiPiaoPrintSt02, CaiPiaoJi.Num02);
    }

    /// <summary>
    /// 玩家是否激活游戏.
    /// </summary>
    [HideInInspector]
	public bool IsPlayerActivePcvr = true;
	float TimeLastActivePcvr;
    /// <summary>
    /// 检测硬件是否被激活.
    /// 动态调整串口通信速度.
    /// </summary>
	void CheckIsPlayerActivePcvr()
	{
		if (Application.loadedLevel >= 1)
        {
            IsPlayerActivePcvr = true;
            return;
		}

		if (!IsPlayerActivePcvr)
        {
			return;
		}
		
		if (Time.realtimeSinceStartup - TimeLastActivePcvr > 60f) {
			IsPlayerActivePcvr = false;
		}
	}
	
    /// <summary>
    /// 激活pcvr.
    /// </summary>
	public void SetIsPlayerActivePcvr()
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
    public void SetCaiPiaoPrintCmd(CaiPiaoPrintCmd printCmd, CaiPiaoJi indexCaiPiaoJi, int caiPiaoCount)
    {
        Debug.Log("SetCaiPiaoPrintState -> printCmd " + printCmd + ", indexCaiPiaoJi " + indexCaiPiaoJi + ", caiPiaoCount " + caiPiaoCount);
        CaiPiaoPrintCmdVal[(int)indexCaiPiaoJi] = printCmd;
        if (printCmd == CaiPiaoPrintCmd.QuanPiaoPrint || printCmd == CaiPiaoPrintCmd.BanPiaoPrint)
        {
            CaiPiaoCountPrint[(int)indexCaiPiaoJi] = caiPiaoCount;
            CaiPiaoJiPrintStArray[(int)indexCaiPiaoJi] = CaiPiaoPrintState.Null;
        }
    }

    /// <summary>
    /// 收到彩票打印状态信息.
    /// </summary>
    void OnReceiveCaiPiaoJiPrintState(CaiPiaoPrintState printSt, CaiPiaoJi indexCaiPiaoJi)
    {
        switch (printSt)
        {
            case CaiPiaoPrintState.WuXiao:
                {
                    if (CaiPiaoJiPrintStArray[(int)indexCaiPiaoJi] != CaiPiaoPrintState.WuXiao)
                    {
                        Debug.Log("CaiPiaoJi_" + indexCaiPiaoJi + " -> print wuXiao!");
                    }

                    if (CaiPiaoCountPrint[(int)indexCaiPiaoJi] > 0)
                    {
                        SetCaiPiaoPrintCmd(CaiPiaoPrintCmd.QuanPiaoPrint, indexCaiPiaoJi, CaiPiaoCountPrint[(int)indexCaiPiaoJi]);
                    }
                    break;
                }
            case CaiPiaoPrintState.Succeed:
                {
                    Debug.Log("CaiPiaoJi_" + indexCaiPiaoJi + " -> print succeed!");
                    SetCaiPiaoPrintCmd(CaiPiaoPrintCmd.StopPrint, indexCaiPiaoJi, 0);
                    if (CaiPiaoJiPrintStArray[(int)indexCaiPiaoJi] != CaiPiaoPrintState.Succeed)
                    {
                        CaiPiaoCountPrint[(int)indexCaiPiaoJi] -= 1;
                    }
                    break;
                }
            case CaiPiaoPrintState.Failed:
                {
                    Debug.Log("CaiPiaoJi_" + indexCaiPiaoJi + " -> print failed!");
                    SetCaiPiaoPrintCmd(CaiPiaoPrintCmd.StopPrint, indexCaiPiaoJi, 0);
                    break;
                }
        }
        CaiPiaoJiPrintStArray[(int)indexCaiPiaoJi] = printSt;
    }

    /// <summary>
    /// 玩家币值索引.
    /// </summary>
    public enum PlayerCoinEnum
    {
        player01 = 0,
        player02 = 1,
        player03 = 2,
        player04 = 3,
    }

    /// <summary>
    /// hid币值信息.
    /// </summary>
    int[] PlayerCoinHidArray = new int[4];
    /// <summary>
    /// 玩家币值信息.
    /// </summary>
    [HideInInspector]
    public int[] PlayerCoinArray = new int[4];
    /// <summary>
    /// 减币.
    /// </summary>
    public void SubPlayerCoin(int subNum, PlayerCoinEnum indexPlayer)
    {
        int indexVal = (int)indexPlayer;
        if (PlayerCoinArray[indexVal] > subNum)
        {
            PlayerCoinArray[indexVal] -= subNum;
        }
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
    /// 电位器数据列表.
    /// </summary>
    [HideInInspector]
    public uint[] DianWeiQiDtArray = new uint[8];
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

    /// <summary>
    /// 获取电位器数据信息.
    /// </summary>
    public uint GetDianWeiQiDt(byte gaoWei, byte diWei)
    {
        return (((uint)gaoWei & 0x0f) << 8) + diWei;
    }

    /// <summary>
    /// 更新电位器数据信息.
    /// </summary>
    void UpdateDianWeiQiDt(byte[] buffer)
    {
        if (CheckADKeyIsError(buffer[46]))
        {
            return;
        }

        DianWeiQiDtArray[0] = GetDianWeiQiDt(buffer[2], buffer[3]);
        DianWeiQiDtArray[1] = GetDianWeiQiDt(buffer[4], buffer[5]);
        DianWeiQiDtArray[2] = GetDianWeiQiDt(buffer[6], buffer[7]);
        DianWeiQiDtArray[3] = GetDianWeiQiDt(buffer[8], buffer[9]);
        DianWeiQiDtArray[4] = GetDianWeiQiDt(buffer[10], buffer[11]);
        DianWeiQiDtArray[5] = GetDianWeiQiDt(buffer[12], buffer[13]);
        DianWeiQiDtArray[6] = GetDianWeiQiDt(buffer[14], buffer[15]);
        DianWeiQiDtArray[7] = GetDianWeiQiDt(buffer[16], buffer[17]);
    }

    /// <summary>
    /// 是否可以检测读取的数据.
    /// </summary>
    bool IsCanCheckReadMsg;
    /// <summary>
    /// 检测获取的IO信息是否错误.
    /// </summary>
    public bool CheckGetMsgInfoIsError(byte[] buffer)
    {
        if (!MyCOMDevice.GetInstance().IsFindDeviceDt)
        {
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