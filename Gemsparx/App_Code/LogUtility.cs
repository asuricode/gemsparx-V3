using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for LogUtility
/// </summary>
public class LogUtility
{
    public static bool blnErrorLog = true;
    public static bool SaveLogEntry(string ErrorMessage)
    {
        try
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append("\r\n");
            sbMessage.Append("\r\n");
            sbMessage.Append("Date --" + System.DateTime.Now);
            sbMessage.Append("\r\n");
            sbMessage.Append("ErrorMessage --" + ErrorMessage);
            sbMessage.Append("\r\n");
            sbMessage.Append("\r\n");
            sbMessage.Append("****************************************************************************************");
            bool flag = WriteToLog(sbMessage);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private static bool WriteToLog(StringBuilder sbMessage)
    {
        try
        {
            FileStream fs;
            string strLogFileName = "Gemsparx_Error_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string strLogFilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs\\";
            if (!Directory.Exists(strLogFilePath))
                Directory.CreateDirectory(strLogFilePath);
            if (File.Exists(strLogFilePath + strLogFileName) == true)
            { fs = File.Open(strLogFilePath + strLogFileName, FileMode.Append, FileAccess.Write); }
            else
            {
                fs = File.Create(strLogFilePath + strLogFileName);
            }
            {
                using (StreamWriter sw = new StreamWriter(fs)) { sw.Write(sbMessage.ToString() + Environment.NewLine); }
                fs.Close();
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}