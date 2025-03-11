// Author: Eve Hill

namespace TapWaterStandardLibrary;
using System.IO;

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

    // Needed to ensure consistency for csv file logging.
    int columnSize = -1;  

    private string messageFormatIndicator = ":v:";

    public void CreateFile(string path, string name)
    {
        FilePath = $"{path}\\{name}.csv";
        File.Create(FilePath); 
    }

    public void WriteString(string message)
    {  
        if (FilePath == string.Empty)
        {
            return;
        } 

        File.WriteAllLines(FilePath, new string[] { message });
    } 

    /// <summary>
    /// Takes an array of messages and formats them to align with CSV standard for printing into log. 
    /// If this is the first write to this log, this stores the size of the column as per CSV convention.
    /// </summary>
    /// <returns> A new formatted string. </returns>
    public string FormatStringForLog(string[] messages)
    {
        string formatted = "";
        int accum = 0;

        foreach (string m in messages)
        {
            //  Size is set         And we're out of range
            if (columnSize != -1 && accum > columnSize)
            {
                // Don't continue printing.
                break;
            }

            formatted = formatted + $"{m},";
            accum++;
        }

        columnSize = accum; 
        return formatted + "\n";
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