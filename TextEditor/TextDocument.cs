using System;
using System.IO;

public class TextDocument

{
    public string Content { get; set; }
    public string FilePath { get; private set; }

    public TextDocument()
    {
        Content = string.Empty;
        FilePath = null;
    }

    public void Open(string filePath)
    {
        Content = File.ReadAllText(filePath);
        FilePath = filePath;
    }

    public void Save()
    {
        if (string.IsNullOrWhiteSpace(FilePath))
        {
            throw new InvalidOperationException("File path must be set before saving.");
        }
        File.WriteAllText(FilePath, Content);
    }

    public void SaveAs(string filePath)
    {
        FilePath = filePath;
        Save();
    }
}
