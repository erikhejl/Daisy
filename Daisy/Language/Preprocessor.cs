namespace Ancestry.Daisy.Language
{
    using System.IO;

    public class Preprocessor
    {
        public static Stream Preprocess(Stream stream)
        {
            var outStream = new MemoryStream();
            var writer = new StreamWriter(outStream); 

            var reader = new StreamReader(stream);
            string line;
            while((line = reader.ReadLine()) != null)
            {
                WriteLine(writer,line);
            }
            writer.Flush();
            outStream.Position = 0;
            return outStream;
        }

        private static StreamWriter WriteLine(StreamWriter s, string str)
        {
            s.Write(str);
            s.Write("\n");
            return s;
        }
    }
}
