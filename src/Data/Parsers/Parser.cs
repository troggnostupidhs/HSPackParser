using System;
using System.Collections.Generic;
using System.IO;
using HSPackParser.Data.Poco;

namespace HSPackParser.Data.Parsers
{
    public class Parser
    {
        public List<Pack> Packs { get; private set; }

        public Parser()
        {
        }

        private Card ParseCard(string cardInfo)
        {
            string[] cardInfoParts = cardInfo.Split(new[] { ' ', ',', '[', ']', '(', ')', '\'' }, StringSplitOptions.RemoveEmptyEntries);
            Card c = new Card
            {
                Id = cardInfoParts[0],
                IsGolden = "1".Equals(cardInfoParts[1]),
            };
            return c;
        }

        private List<Card> ParseCards(string line)
        {
            var cards = new List<Card>();

            string[] cardInfos = line.Split(new[] { "), (" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cardInfo in cardInfos)
            {
                cards.Add(ParseCard(cardInfo));
            }

            return cards;
        }

        private Pack ParsePack(string line)
        {
            string[] parts = line.Split(new[] { ",\"", "\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] packInfo = parts[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<Card> cards = ParseCards(parts[1]);

            Pack p = new Pack()
            {
                Id = packInfo[0],
                BoosterType = packInfo[1],
                OpenDate = packInfo[2],
                UploaderId = packInfo[3],
                AccountId = packInfo[4],
                RegionId = packInfo[5],
                Cards = cards,
            };
            return p;
        }

        public List<Pack> Parse(string file)
        {
            Packs = new List<Pack>();

            FileStream fileStream = new FileStream(file, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Packs.Add(ParsePack(line));
                }
            }

            return Packs;
        }
    }
}