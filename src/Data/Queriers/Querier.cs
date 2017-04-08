using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HSPackParser.Data.Poco;

namespace HSPackParser.Data.Queriers
{
    public class Querier
    {
        public List<Pack> Packs { get; private set; }
        private List<Card> AllCards { get; set; }

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
            Dictionary<string, int> cardCount = new Dictionary<string, int>();
            foreach (Card c in AllCards)
            {
                string key = c.Id;
                int currentCount = cardCount.ContainsKey(key) ? cardCount[key] : 0;
                currentCount++;
                cardCount[key] = currentCount;
            }

            var sorted = cardCount.OrderBy(kp => kp.Key).ToList();
            foreach (var kvp in sorted)
            {
                string output = string.Format("{0} {1}{2}", kvp.Key, kvp.Value, Environment.NewLine);
                Console.Write(output);
                //File.AppendAllText("out.txt", output);
            }
        }

        public void Run()
        {
            PrintByCardCounts();
        }
    }
}