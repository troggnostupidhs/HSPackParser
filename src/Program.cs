using HSPackParser.Data.Parsers;
using HSPackParser.Data.Queriers;
using HSPackParser.src.Data.CardInfos;

namespace HSPackParser
{
    public class Program
    {
        public const string FILE_NAME = @"resources\31YYEWQ.txt";

        public static void Main(string[] args)
        {
            Parser p = new Parser();
            p.Parse(FILE_NAME);
            CardInfoGetter.GetCardInfo(p.Packs);

            Querier q = new Querier(p.Packs);
            q.Run();
        }
    }
}
