using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using NotVisualBasic.FileIO;

namespace SourceGeneratorLib.Generators
{
    [Generator]
    public class CsvGenerator : ISourceGenerator
    {

        public enum CsvLoadType
        {
            Startup,
            OnDemand
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            IEnumerable<(CsvLoadType, bool, AdditionalText)> options = GetLoadOptions(context);
            IEnumerable<(string, string)> nameCodeSequence = SourceFilesFromAdditionalFiles(options);
            foreach ((string name, string code) in nameCodeSequence)
                context.AddSource($"Csv_{name}", SourceText.From(code, Encoding.UTF8));
        }

        private IEnumerable<(string, string)> SourceFilesFromAdditionalFiles(
            IEnumerable<(CsvLoadType loadType, bool cacheObjects, AdditionalText file)> options)
        {
            return options.SelectMany(x => SourceFilesFromAdditionalFile(x.loadType, x.cacheObjects, x.file));
        }

        private IEnumerable<(string, string)> SourceFilesFromAdditionalFile(CsvLoadType loadType, bool cacheObjects, AdditionalText file)
        {
            var className = Path.GetFileNameWithoutExtension(file.Path);
            var csvText = file.GetText()!.ToString();
            return new (string, string)[]{ (className, GenerateClassFile(className, csvText, loadType, cacheObjects))};
        }

        private string GenerateClassFile(string className, string csvText, CsvLoadType loadType, bool cacheObjects)
        {
            var builder = new StringBuilder();
            using var parser = new CsvTextFieldParser(new StringReader(csvText));

            //// Usings
            builder.Append(@"
#nullable enable
using System.Collections.Generic;
namespace NetConf2020 {
");
            //// Class Definition
            builder
.Append($"    public class {className} {{\n");


            if (loadType == CsvLoadType.Startup)
            {
                builder
.Append(@$"
        static {className}() {{ var x = All; }}
");
            }
            (string[] types, string[] names, string[]? fields) = ExtractProperties(parser);
            int minLen = Math.Min(types.Length, names.Length);

            for (int i = 0; i < minLen; i++)
            {
                builder
.AppendLine($"        public {types[i]} {StringToValidPropertyName(names[i])} {{ get; set;}} = default!;");
            }
            builder
.Append("\n");

            //// Loading data
            builder
.AppendLine($"        static IEnumerable<{className}>? _all = null;");
            builder
.Append($@"
        public static IEnumerable<{className}> All {{
            get {{");

            if (cacheObjects) builder
.Append(@"
                if(_all != null)
                    return _all;
");
            builder
.Append(@$"

                List<{className}> l = new List<{className}>();
                {className} c;
");

            // This awkwardness comes from having to pre-read one row to figure out the types of props.
            do
            {
                if (fields == null) continue;
                if (fields.Length < minLen) throw new Exception("Not enough fields in CSV file.");

                builder
.AppendLine($"                c = new {className}();");
                string value = "";
                for (int i = 0; i < minLen; i++)
                {
                    // Wrap strings in quotes.
                    value = GetCsvFieldType(fields[i]) == "string" ? $"\"{fields[i].Trim().Trim(new char[] { '"' })}\"" : fields[i];
                    builder
.AppendLine($"                c.{names[i]} = {value};");
                }
                builder
.AppendLine("                l.Add(c);");

                fields = parser.ReadFields();
            } while (!(fields == null));

            builder
.AppendLine("                _all = l;");
            builder
.AppendLine("                return l;");

            // Close things (property, class, namespace)
            builder
.Append("            }\n        }\n    }\n}\n");
            return builder
.ToString();


        }

        public static (string[], string[], string[]?) ExtractProperties(CsvTextFieldParser parser)
        {
            string[]? headerFields = parser.ReadFields();
            if (headerFields == null) throw new Exception("Empty csv file!");

            string[]? firstLineFields = parser.ReadFields();
            if (firstLineFields == null)
            {
                return (Enumerable.Repeat("string", headerFields.Length).ToArray(), headerFields, firstLineFields);
            }
            else
            {
                return (firstLineFields.Select(GetCsvFieldType).ToArray(), headerFields.Select(StringToValidPropertyName).ToArray(), firstLineFields);
            }
        }

        public static string GetCsvFieldType(string exemplar) => exemplar switch
        {
            _ when bool.TryParse(exemplar, out _) => "bool",
            _ when int.TryParse(exemplar, out _) => "int",
            _ when double.TryParse(exemplar, out _) => "double",
            _ => "string"
        };

        public static string StringToValidPropertyName(string s)
        {
            s = s.Trim();
            s = char.IsLetter(s[0]) ? char.ToUpper(s[0]) + s.Substring(1) : s;
            s = char.IsDigit(s.Trim()[0]) ? "_" + s : s;
            s = new string(s.Select(ch => char.IsDigit(ch) || char.IsLetter(ch) ? ch : '_').ToArray());
            return s;
        }

        private IEnumerable<(CsvLoadType, bool, AdditionalText)> GetLoadOptions(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                if (Path.GetExtension(file.Path).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.additionalfiles.csvloadtype", out var loadTypeStr);
                    Enum.TryParse(loadTypeStr, true, out CsvLoadType loadType);

                    context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.additionalfiles.cacheobjects", out var cacheObjectsStr);
                    bool.TryParse(cacheObjectsStr, out bool cacheObjects);

                    yield return (loadType, cacheObjects, file);
                }
            }
        }
    }
}