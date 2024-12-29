using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FuncTools;
using TagCloud.WordsReader.Settings;

namespace TagCloud.WordsReader.Readers;

public class CsvFileReader(string filePath, CultureInfo cultureInfo) : BaseFileReader(filePath)
{
    private class TableCell
    {
        [Index(0)]
        public string Word { get; set; }
    }
    
    public CsvFileReader(CsvFileReaderSettings settings)
        : this(settings.FilePath, settings.Culture)
    { }

    protected override Result<List<string>> ReadFromExistingFile(string path) 
    {
        var configuration = new CsvConfiguration(cultureInfo)
        {
            HasHeaderRecord = false
        };
        
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, configuration);
        return csv.GetRecords<TableCell>().Select(cell => cell.Word).ToList();
    }
}