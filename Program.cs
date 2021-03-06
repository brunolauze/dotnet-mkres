using System;
using System.Collections.Generic;
using System.Resources;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.DotNet.Tools.Resgen
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            if (args.Length == 0) return;
            var vals = args[0].Split(',');
            if (vals.Length != 2) return;
            var sourceFile = new FileInfo(vals[0]);
            var outputFile = new FileInfo(vals[1]);
            using (var outputStream = outputFile.Create())
            {
              using (var input = sourceFile.OpenRead())
              {
                  var document = XDocument.Load(input);
                  var data = document.Root.Elements("data");
                  if (data.Any())
                  {
                      var rw = new ResourceWriter(outputStream);
                      var keys = new List<string>();
                      foreach (var e in data)
                      {
                          var name = e.Attribute("name").Value;
                          var value = e.Element("value").Value;
                          if (keys.Contains(name)) continue;
                          keys.Add(name);
                          rw.AddResource(name, value);
                      }
  
                      rw.Generate();
                  }
               }
            }
        }
    }
}
