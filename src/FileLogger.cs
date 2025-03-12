// Author: Eve Hill 03/12/2025

namespace TapWaterStandardLibrary;
using Godot;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FileLogger
{
    private string filePath = string.Empty;
    public string FilePath { 
        get { return filePath; }
        private set {
            // Never overwrite an existing file path unless explicitly called by the developer.
            if (filePath == string.Empty)
            {
                filePath = value;
            } 
            return;
        } 
    }     

    /// <summary>
    /// Takes a formated string (see FileLogger.FormatStringFromLog) and writes it to the current log file. 
    /// Only pass a path on first write. If there is no path an exception will be thrown. 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="path"></param>
    public void WriteString(string message, string path = "")
    { 

        if (path != "" && FilePath != string.Empty)
        {
            throw new Exception("File path alrady set, cannot set it again without explicit override.");
        }

        if (path != "")
        {
            FilePath = path;
        }

        if (FilePath == string.Empty)
        {
            throw new Exception("No file path set! Cannot write to log!");
        } 


        if (!File.Exists(FilePath))
        {
            try 
            {                    
                GD.Print("NO file exists, creating new one.");
                // Create New File
                using (FileStream fs = File.Create(FilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(message);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception e)
            {
                GD.PrintErr(e.ToString());
            }

            return;
        }

        try
        {
            GD.Print("File exits, writing into it.");
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Open(FilePath, FileMode.Open))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(message);
                // Add some information to the file.
                fs.Seek(0, SeekOrigin.End);
                fs.Write(info, 0, info.Length);
            }

        } 
        catch (Exception e)
        {
            GD.PrintErr(e.ToString());
        }  
    } 

    /// <summary>
    /// Takes an array of messages and formats them to align with CSV standard for printing into log. 
    /// If this is the first write to this log, this stores the size of the column as per CSV convention.
    /// </summary>
    /// <returns> A new formatted string. </returns>
    public string FormatStringForLog(List<string> messages)
    {
        string formatted = "";
        int accum = 0;

        foreach (string m in messages)
        {  
            //  Size is set         And we're out of range
            if (columnSize != -1 && accum >= columnSize)
            {
                // Don't continue printing.
                break;
            }

            // If last item, don't add a comma
            if (accum == messages.Count - 1)
            {
                formatted = formatted + $"{m}";
                accum++;
                break;                
            } 

            formatted = formatted + $"{m},";
            accum++; 
        }

        columnSize = messages.Count;
        return formatted + "\r\n";

    } 

    /// <summary>
    /// Overwites the path to the log file. This should rarely be used.
    /// </summary>
    /// <param name="path"></param>      
    public void OverwriteFilePath(string path)
    {
        filePath = path;
    }

}