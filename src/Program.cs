using HSPackParser.Data;

namespace HSPackParser
{
    public class Program
    {
        public const string FILE_NAME = "31YYEWQ.txt";

        public static void Main(string[] args)
        {
            Parser p = new Parser();
            p.Parse(FILE_NAME);
        }
    }
}
