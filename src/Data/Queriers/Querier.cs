using HSPackParser.Data.Poco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HSPackParser.Data.Queriers
{
    /// <summary>
    /// Run queries.
    /// </summary>
    public class Querier
    {
        /// <summary>
        /// List of packs being used.
        /// </summary>
        public List<Pack> Packs { get; private set; }

        private List<Card> AllCards { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="packs">List of Packs.</param>
        public Querier(List<Pack> packs)
        {
            Packs = packs;
            AllCards = packs.Select((p) => p.Cards)
                            .Aggregate(new List<Card>(), (result, cards) =>
                            {
                                result.AddRange(cards);
                                return result;
                            }).ToList();
        }

        private void PrintByCardCounts()
        {
            var cardCount = new Dictionary<HearthDb.Card, int>();
            foreach (Card card in AllCards)
            {
                HearthDb.Card key = card.CardInfo;
                int currentCount = cardCount.ContainsKey(key) ? cardCount[key] : 0;
                currentCount++;
                cardCount[key] = currentCount;
            }

            var sorted = cardCount.OrderByDescending(kp => kp.Value).GroupBy(kp => kp.Key.Rarity).ToList();
            foreach (var groups in sorted)
            {
                WriteLine("{0}", groups.Key);
                WriteLine("------------------------------");
                groups.ToList().ForEach((kvp) =>
                {
                    WriteLine("{0,25} {1}", kvp.Key.Name, kvp.Value);
                });
                WriteLine("");
            }
        }

        private void Write(String format, params object[] args)
        {
            string str = string.Format(format, args);
            Console.Write(str);
            File.AppendAllText("out.txt", str);
        }

        private void WriteLine(String format, params object[] args)
        {
            Write(format, args);
            Write("{0}", Environment.NewLine);
        }

        /// <summary>
        /// Runs the queries.
        /// </summary>
        public void Run()
        {
            try
            {
                File.Delete("out.txt");
            }
            catch { }

            PrintByCardCounts();
        }
    }
}