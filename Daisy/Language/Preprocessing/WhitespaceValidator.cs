using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Language.Preprocessing
{
    public class InconsistentWhitespaceException : Exception
    {
        public int LineNumber { get; set; }
        public string Line { get; set; }
        public char UnexpectedWhitespace { get; set; }
    }

    public static class WhitespaceValidator
    {
        public static bool Validate(string[] codelines)
        {
            var whitespace = codelines
                .SelectMany((i, c) => Regex.Match(i, "^[ \t]*").Value
                    .Select(k => new { line = c + 1, @char = k }))
                .GroupBy(i => i.@char)
                .Select(i => new { @char = i.Key, firstLine = i.Min(k => k.line) })
                .ToList();

            if (whitespace.Count > 1)
            {
                var badLine = whitespace.OrderBy(k => k.firstLine).Skip(1).First();
                throw new InconsistentWhitespaceException
                {
                    LineNumber = badLine.firstLine,
                    Line = codelines[badLine.firstLine],
                    UnexpectedWhitespace = badLine.@char
                };
            }
            return true;
        }
    }
}
