﻿using FuncTools;

namespace TagCloud.WordsReader.Readers;

public abstract class BaseFileReader(string path) : IWordsReader
{
    public Result<List<string>> ReadWords()
    {
        return File.Exists(path) 
            ? ReadFromExistingFile(path) 
            : Result.Fail<List<string>>("No file found");
    }

    public abstract Result<List<string>> ReadFromExistingFile(string path);
}