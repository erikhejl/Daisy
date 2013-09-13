namespace Ancestry.Daisy.Documentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using Ancestry.Daisy.Documentation.Utils;
    using Ancestry.Daisy.Utils;

    public interface ICommentDocumentation
    {
        MethodDocumentation ForMethod(MethodInfo methodInfo);
    }

    public class CommentDocumentation : ICommentDocumentation
    {
        private XDocument document;

        private Dictionary<string, MethodDocumentation> methodDocs;

        public CommentDocumentation(Stream stream)
        {
            document = XDocument.Load(stream);
            Parse();
        }

        public CommentDocumentation(string fileName)
        {
            using (var stream  = new FileStream(fileName, FileMode.Open))
            {
                document = XDocument.Load(stream);
            }
            Parse();
        }

        private void Parse()
        {
            methodDocs = document
                .Descendants()
                .Where(x =>
                    x.Name == "member"
                    && x.Attributes().Any(y => y.Name == "name" && y.Value.StartsWith("M:"))
                )
                .Select(x => new MethodDocumentation()
                    {
                        MethodName = x.Attributes()
                            .First(y => y.Name == "name")
                            .Value
                            .Substring(2),
                        Summary = x.Descendants()
                            .FirstOrDefault(y => y.Name == "summary")
                            .With(y => y.Value)
                            .With(y => y.ConsolidateWhitespace()),
                        Parameters = x.Descendants()
                                      .Where(y => y.Name == "param")
                                      .Select(e => new {
                                          name = e.Attributes()
                                            .FirstOrDefault(a => a.Name == "name")
                                            .With(a => a.Value),
                                          description = e.Value
                                      })
                                      .Where(a => !string.IsNullOrEmpty(a.name))
                                      .ToDictionary(z => z.name, z => z.description)
                    })
                .ToDictionary(x => x.MethodName, x => x);
        }

        public MethodDocumentation ForMethod(MethodInfo methodInfo)
        {
            return methodDocs.With(methodInfo.GetDocStyleSignature());
        }
    }

    public class MethodDocumentation
    {
        public string MethodName { get; set; }
        public string Summary { get; set; }
        public IDictionary<string,string> Parameters { get; set; }
    }
}
