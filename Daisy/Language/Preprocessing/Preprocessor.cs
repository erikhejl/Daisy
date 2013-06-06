namespace Ancestry.Daisy.Language.Preprocessing
{
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;

    public class Preprocessor
    {
        public static Stream Preprocess(Stream stream)
        {
            var outStream = new MemoryStream();
            var writer = new StreamWriter(outStream);
            using (var reader = new StreamReader(stream))
            {
                var lines = ReadLines(reader).ToArray();
                WhitespaceValidator.Validate(lines);
                foreach(var line in lines)
                    WriteLine(writer, line);
            }
            
            writer.Flush();
            outStream.Position = 0;
            return outStream;
        }

        private static IEnumerable<string> ReadLines(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;    
            }
        }
        private static StreamWriter WriteLine(StreamWriter s, string str)
        {
            s.Write(str);
            s.Write("\n");
            return s;
        }
    }
}
