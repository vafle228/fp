﻿using FuncTools;
using TagCloud.WordsReader.Settings;
using Xceed.Words.NET;

namespace TagCloud.WordsReader.Readers;

public class WordFileReader(string filePath) : BaseFileReader(filePath)
{
    public WordFileReader(WordFileReaderSettings settings)
        : this(settings.FilePath)
    { }
    
    public override Result<List<string>> ReadFromExistingFile(string path) 
    {
        using var document = DocX.Load(path);
        var paragraphs = document.Paragraphs;
        return paragraphs.Select(p => p.Text).ToList();
    }
}