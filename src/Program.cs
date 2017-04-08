using HSPackParser.Data.Parsers;
using HSPackParser.Data.Queriers;

namespace HSPackParser
{
    public class Program
    {
        public const string FILE_NAME = "31YYEWQ.txt";

        public static void Main(string[] args)
        {
            var test = HearthDb.Cards.All[""];

            Parser p = new Parser();
            p.Parse(FILE_NAME);

            Querier q = new Querier(p.Packs);
            q.Run();
        }
    }
}
