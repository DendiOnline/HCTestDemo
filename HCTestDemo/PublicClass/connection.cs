/*==========================================================
 Copyright (C) 2005 YOKOGAWA ELECTRIC CORPORATION

    ALL RIGHTS RESERVED BY YOKOGAWA ELECTRIC CORPORATION.
THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT
WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING
BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY
AND/OR FITNESS FOR A PARTICULAR PURPOSE.

                            YOKOGAWA ELECTRIC CORPORATION
==========================================================*/
using TmctlAPINet;
using System.Data;
using System.Text;

namespace Yokogawa.Tm.WT1800CommSample.cs
{
  /// <summary>
  /// WT1800 Communication Sample Program, Connection Class.
  /// </summary>
  public class Connection
  {
    #region Custem Member Init
    private static int commID = -1;
    private static string model = "";
    private string address;      //address parameter.
    private int commType;     //connection type.
    private int terminator;      //terminator in message.
    private static TMCTL tmDev = new TMCTL();
    public static string DevModel
    {
      get
      {
        return model;
      }
    }
    public string devAddress
    {
      set
      {
        address = value;
      }
    }
    public int wireType
    {
      set
      {
        commType = value;
      }
    }
    public int msgTerminator
    {
      set
      {
        terminator = value;
      }
    }
    
    #endregion

    #region Constructor
    public Connection()
    {
      //tmDev = new TmctlDevice();
      terminator = 2;
    }

    public Connection(int port, string addr)
    {
      commType   = port;
      address    = addr;
      terminator = 1;
    }
    #endregion

    #region Function: Initialize
    //**************************************************
    /// <summary>
    /// Set Connection To The Device
    /// </summary>
    /// <returns></returns>
    //**************************************************
    public int Initialize()
    {
      int rtn;//return 0 when successed.
      rtn = tmDev.Initialize(commType, address, ref commID);
      if(rtn != 0)
      {
        return rtn;
      }
  	
      //set terminator of the message.
      rtn = tmDev.SetTerm(commID, terminator, 1);
      if(rtn != 0)
      {
        tmDev.Finish(commID);
        return rtn;
      }

      //timeout settings, 100*100ms
      rtn = tmDev.SetTimeout(commID, 100);
      if(rtn != 0)
      {
        tmDev.Finish(commID);
        return rtn;
      }

      //test the device module connected.
      rtn = tmDev.Send(commID, "*IDN?");

      int maxLength = 50;
      StringBuilder buf;
      int realLength = 0;
      buf = new StringBuilder(20000);

      rtn = tmDev.Receive(commID, buf, maxLength, ref realLength);
      model = buf.ToString();
      if(rtn != 0)
      {
        //it seems no use to do a finish when rtn != 0.
        tmDev.Finish(commID);
      }
      return rtn;
    }
    #endregion
    
    #region Function: SetTimeout
    //**************************************************
    /// <summary>
    /// Function: Set Timeout Method
    /// </summary>
    /// <returns></returns>
    //**************************************************
    public int SetTimeout(int timeout)
    {
      return(tmDev.SetTimeout(commID, timeout));
    }
    #endregion

    #region Function: Finish
    //**************************************************
    /// <summary>
    /// Function: Break the Connection Method
    /// </summary>
    /// <returns></returns>
    //**************************************************
    public int Finish()
    {
      return(tmDev.Finish(commID));
    }
    #endregion

    #region Function: Send
    //**************************************************
    /// <summary>
    /// Function: Sending Message Method
    /// </summary>
    /// <returns></returns>
    //**************************************************
    public int Send(string msg)
    {
      return(tmDev.Send(commID, msg));
    }
    public int SendByLength(string msg, int len)
    {
      return(tmDev.SendByLength(commID, msg, len));
    }
    #endregion

    #region Function: Receive
    //**************************************************
    /// <summary>
    /// Function: Receive Message
    /// </summary>
    /// <returns></returns>
    //**************************************************
    public int ReceiveSetup()
    {
      return(tmDev.ReceiveSetup(commID));
    }

    public int Receive(ref string buf, int blen, ref int rlen)
    {
      StringBuilder temp;
      temp = new StringBuilder(41000);
      int rtn = tmDev.Receive(commID, temp, blen, ref rlen);
      buf = temp.ToString();
      return rtn;
    }

    public int ReceiveBlockHeader(ref int rlen)
    {
      return(tmDev.ReceiveBlockHeader(commID, ref rlen));
    }
    public int ReceiveBlockData(ref byte[] buf, int blen, ref int rlen, ref int end)
    {
      int rtn = tmDev.ReceiveBlockData(commID, ref buf[0], blen, ref rlen, ref end);
      return rtn;
    }
    #endregion

    #region Function: GetLastError
    //**************************************************
    /// <summary>
    /// Function: Get Last Error Method
    /// </summary>
    /// <returns></returns>
    //**************************************************
    public int GetLastError()
    {
      return(tmDev.GetLastError(commID));
    }
    #endregion

    #region Function: SetRen
    //**************************************************
    /// <summary>
    /// Function: Set the Remote Status
    /// </summary>
    /// <returns></returns>
    //**************************************************
    //##Function: Set the Remote Status##
    public int SetRen(int flag)
    {
      return(tmDev.SetRen(commID, flag));
    }
    #endregion

    #region Function: GetEncodeSerialNumber
    //**************************************************
    /// <summary>
    /// Function: Get the EncodeSerialNumber
    /// </summary>
    /// <returns></returns>
    //**************************************************
     public int GetEncodeSerialNumber(StringBuilder decode, int len, string src)
    {
        return (tmDev.EncodeSerialNumber(decode, len, src));
    }
    #endregion
  }
}
