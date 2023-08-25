﻿namespace eRS.Models.Models.Files;

public class CsvFile
{
    public string FileContents { get; set; }
    public string FileName { get; set; }

    public CsvFile(string fileContents, string fileName)
    {
        this.FileContents = fileContents;
        this.FileName = fileName;
    }
}

