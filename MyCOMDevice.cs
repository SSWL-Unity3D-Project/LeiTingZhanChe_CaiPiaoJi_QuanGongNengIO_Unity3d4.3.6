﻿using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.IO.Ports;

public class MyCOMDevice : MonoBehaviour
{
    public class ComThreadClass
	{
		//string ThreadName;
        static SerialPort _SerialPort;
        public static int BufLenRead = 60;
		public static  int BufLenWrite = 50;
		public static byte[] ReadByteMsg = new byte[BufLenRead];
		public static byte[] WriteByteMsg = new byte[BufLenWrite];
		int ReadTimeout = 0x00064; //单位为毫秒.
		int WriteTimeout = 0x07d0; //单位为毫秒.
        public static string ComPortName = "COM1";
		//public static int WriteCount;
		public static int ReadCount;

		public ComThreadClass(string name)
		{
			//ThreadName = name;
			OpenComPort();
		}

        /// <summary>
        /// 打开串口.
        /// </summary>
        public void OpenComPort()
		{
			if (_SerialPort != null) {
				return;
			}

            _SerialPort = new SerialPort(ComPortName, 57600, Parity.None, 8, StopBits.One);
			if (_SerialPort != null)
			{
				try
				{
					if (_SerialPort.IsOpen)
					{
						_SerialPort.Close();
						Debug.Log("Closing port, because it was already open!");
					}
					else
					{
                        _SerialPort.ReadTimeout = ReadTimeout;
                        _SerialPort.WriteTimeout = WriteTimeout;
                        _SerialPort.Open();
						if (_SerialPort.IsOpen) {
                            if (_Instance != null)
                            {
                                _Instance.IsFindDeviceDt = true;
                            }
							Debug.Log("COM open sucess");
                        }
					}
				}
				catch (Exception exception)
				{
					Debug.Log("error:COM already opened by other PRG... " + exception);
				}
			}
			else
			{
				Debug.Log("Port == null");
			}
		}

        /// <summary>
        /// 线程运行函数.
        /// </summary>
		public void Run()
		{
			do
            {
                COMTxData();
                //if (pcvr.IsJiaoYanHid || !pcvr.IsPlayerActivePcvr)
                //if (pcvr.IsJiaoYanHid)
                //{
                //    Thread.Sleep(100);
                //}
                //else
                //{
                //    Thread.Sleep(15);
                //}
                COMRxData();
                //if (pcvr.IsJiaoYanHid || !pcvr.IsPlayerActivePcvr)
                if (pcvr.IsJiaoYanHid)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(15);
                }
            }
			while (_SerialPort.IsOpen);
			CloseComPort();
			Debug.Log("Close run thead...");
		}

        /// <summary>
        /// 写串口数据.
        /// </summary>
		void COMTxData()
		{
			try
            {
				_SerialPort.Write(WriteByteMsg, 0, WriteByteMsg.Length);
				//WriteCount++;
			}
			catch (Exception exception)
			{
				Debug.Log("Tx error:COM!!! " + exception);
			}
		}

        /// <summary>
        /// 读串口数据.
        /// </summary>
		void COMRxData()
		{
			try
			{
                _SerialPort.Read(ReadByteMsg, 0, ReadByteMsg.Length);
				ReadCount++;
			}
			catch (Exception exception)
			{
				Debug.Log("Rx error:COM..." + exception);
			}
		}

        /// <summary>
        /// 关闭串口.
        /// </summary>
		public void CloseComPort()
		{
			//IsReadComMsg = false;
			if (_SerialPort == null || !_SerialPort.IsOpen) {
				return;
			}
			_SerialPort.Close();
			_SerialPort = null;
		}
	}

    /// <summary>
    /// 串口线程控制.
    /// </summary>
    ComThreadClass mComThreadClass;
    /// <summary>
    /// 串口IO线程.
    /// </summary>
	Thread mComThreadIO;
    /// <summary>
    /// 是否找到串口设备.
    /// </summary>
    [HideInInspector]
	public bool IsFindDeviceDt;
	static MyCOMDevice _Instance;
    public static MyCOMDevice GetInstance()
	{
		if (_Instance == null) {
			GameObject obj = new GameObject("_MyCOMDevice");
			DontDestroyOnLoad(obj);
			_Instance = obj.AddComponent<MyCOMDevice>();
		}
		return _Instance;
	}
    
    // Use this for initialization
    void Start()
    {
        if (pcvr.bIsHardWare)
        {
            StartCoroutine(OpenComThread());
        }
	}

    /// <summary>
    /// 打开IO线程.
    /// </summary>
	IEnumerator OpenComThread()
	{
		if (mComThreadClass == null) {
			mComThreadClass = new ComThreadClass(ComThreadClass.ComPortName);
		}
		else {
            mComThreadClass.CloseComPort();
		}
		
		if (mComThreadIO != null) {
			CloseComThread();
		}
		yield return new WaitForSeconds(2f);
        
		if (mComThreadIO == null) {
			mComThreadIO = new Thread(new ThreadStart(mComThreadClass.Run));
			mComThreadIO.Start();
		}
	}

    /// <summary>
    /// 关闭IO线程.
    /// </summary>
	void CloseComThread()
	{
		if (mComThreadIO != null) {
			mComThreadIO.Abort();
			mComThreadIO = null;
		}
	}

    /// <summary>
    /// 当程序关闭时.
    /// </summary>
	void OnApplicationQuit()
	{
        Debug.Log("OnApplicationQuit...Com");
        if (mComThreadClass != null)
        {
            mComThreadClass.CloseComPort();
        }
        CloseComThread();
    }
}