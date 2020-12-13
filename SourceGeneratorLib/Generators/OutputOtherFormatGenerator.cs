//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.Text;

//namespace SourceGeneratorLib.Generators
//{
//    [Generator]
//    class OutputOtherFormatGenerator : ISourceGenerator
//    {
//        public void Initialize(GeneratorInitializationContext context)
//        {
//            Debugger.Launch();
//        }

//        /// <summary>
//        /// [測試] : 是否透過 AddSource 輸出 .cs 以外的格式檔案
//        /// [結果] : 不行，AddSource 預設就會將檔案存成 .cs
//        /// [補充] : 如果硬要將檔案輸出成其他格式儲存到某個位置，要從 meta 拿 path
//        /// </summary>
//        /// <param name="context"></param>
//        public void Execute(GeneratorExecutionContext context)
//        {
//            var text = "test hello wolrd.";

//            //C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\Roslyn\
//            //var path = AppDomain.CurrentDomain.BaseDirectory;

//            var path = @"C:\Users\Roberson\Desktop\Playground";
//            using var writer = new StreamWriter(Path.Combine(path, "Hello.txt"));
//            writer.Write(text);
//        }
//    }
//}
