using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class VdfParser
{
    public Dictionary<string, object> ParseFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File {0} not found.", filePath);
        }

        var data = new Dictionary<string, object>();

        using (var reader = new StreamReader(filePath))
        {
            ParseObject(reader, data);
        }

        return data;
    }

    private void ParseObject(StreamReader reader, Dictionary<string, object> container)
    {
        string line;
        string currentKey = null;

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                continue;

            if (line.StartsWith("{"))
            {
                if (currentKey != null)
                {
                    var subObject = new Dictionary<string, object>();
                    ParseObject(reader, subObject);
                    container[currentKey] = subObject;
                    currentKey = null;
                }
            }
            else if (line.StartsWith("}"))
            {
                return;
            }
            else
            {
                var match = Regex.Match(line, "\"(.*?)\"\\s+\"(.*?)\"");
                if (match.Success)
                {
                    currentKey = match.Groups[1].Value;
                    var value = match.Groups[2].Value;
                    container[currentKey] = value;
                }
                else
                {
                    currentKey = line.Replace("\"", "");
                }
            }
        }
    }
}