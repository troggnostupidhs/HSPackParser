using System;
using System.Collections.Generic;
using System.Linq;

namespace HSPackParser.Data
{
    public class Querier
    {
        public List<Pack> Packs { get; private set; }
        private List<Card> AllCards { get; set; }

        public Querier(List<Pack> packs)
        {
            Packs = packs;
            AllCards = new List<Card>();
            foreach (Pack p in Packs)
            {
                AllCards.AddRange(p.Cards);
            }
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

            var sorted = cardCount.OrderByDescending(kp => kp.Key).ToList();
            foreach (var kvp in sorted)
            {
                Console.WriteLine(string.Format("{0} {1}", kvp.Key, kvp.Value));
            }
        }

        public void Run()
        {
            PrintByCardCounts();
        }
    }
}