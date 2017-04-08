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

        private List<Pack> GetPacks()
        {
            var newPacks = new List<Pack>();
            newPacks.AddRange(Packs);
            return newPacks;
        }

        private List<Card> GetAllCards()
        {
            var newAllCards = new List<Card>();
            newAllCards.AddRange(AllCards);
            return newAllCards;
        }

        private void PrintByCardCounts()
        {
            List<Pack> packs = GetPacks();
            List<Card> allCards = GetAllCards();

            var cardCount = new Dictionary<HearthDb.Card, int>();
            allCards.ForEach((card) =>
            {
                HearthDb.Card key = card.CardInfo;
                int currentCount = cardCount.ContainsKey(key) ? cardCount[key] : 0;
                currentCount++;
                cardCount[key] = currentCount;
            });

            var sorted = cardCount.OrderByDescending(kp => kp.Value).GroupBy(kp => kp.Key.Rarity).ToList();
            foreach (var group in sorted)
            {
                var kvps = group.ToList();

                WriteLine("{0} - {1}", group.Key, kvps.Count());
                WriteLine("------------------------------");
                kvps.ForEach((kvp) =>
                {
                    WriteLine("{0,25} {1}", kvp.Key.Name, kvp.Value);
                });
                WriteLine("");
            }
        }

        private void PrintDupesByUser()
        {
            List<Pack> packs = GetPacks();
            List<Card> allCards = GetAllCards();

            var outputs = new Dictionary<HearthDb.Enums.Rarity, List<String>>();
            packs.GroupBy(pack => pack.UploaderId).ToList().ForEach((group) =>
            {
                var packsCount = group.ToList().Count();
                WriteLine("------------------------------");
                WriteLine("{0} - {1}", group.Key, packsCount);
                WriteLine("------------------------------");
                WriteLine("");
                var allUserCards = group.Select((p) => p.Cards)
                    .Aggregate(new List<Card>(), (result, cards) =>
                    {
                        result.AddRange(cards);
                        return result;
                    }).ToList();

                var cardCount = new Dictionary<HearthDb.Card, int>();
                allUserCards.ForEach((card) =>
                {
                    HearthDb.Card key = card.CardInfo;
                    int currentCount = cardCount.ContainsKey(key) ? cardCount[key] : 0;
                    currentCount++;
                    cardCount[key] = currentCount;
                });

                var sorted = cardCount.OrderByDescending(kp => kp.Value).GroupBy(kp => kp.Key.Rarity).ToList();
                foreach (var g in sorted)
                {
                    var kvps = g.ToList();
                    var total = kvps.Sum(kp => kp.Value);

                    WriteLine("{0} - {1} - {2}", g.Key, kvps.Count(), total);
                    WriteLine("------------------------------");
                    WriteLine("{0:0.00}% - {1:0.00}%", ((double)kvps[kvps.Count() - 1].Value / (double)total) * 100.0d, ((double)kvps[0].Value / (double)total) * 100.0d);

                    if (!outputs.ContainsKey(g.Key))
                    {
                        outputs[g.Key] = new List<string>();
                    }
                    outputs[g.Key].Add(string.Format("{0:00.00}%", (double)kvps[0].Value / (double)kvps[kvps.Count() - 1].Value));
                    //kvps.ForEach((kvp) =>
                    //{
                    //    WriteLine("{0,25} {1}", kvp.Key.Name, kvp.Value);
                    //});
                    WriteLine("");
                }
            });

            WriteLine("------------------------------");
            WriteLine("------------------------------");
            WriteLine("");
            outputs.ToList().ForEach((kvp) =>
            {
                WriteLine("{0} ", kvp.Key);
                WriteLine("------------------------------");
                kvp.Value.OrderByDescending(v => v).ToList().ForEach((v) =>
                {
                    WriteLine(v);
                });
                WriteLine("");
            });
        }

        private void Write(String format, params object[] args)
        {
            string str = string.Format(format, args);
            //Console.Write(str);
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
            PrintDupesByUser();
        }
    }
}