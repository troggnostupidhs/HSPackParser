using HSPackParser.Data.Poco;
using System.Collections.Generic;
using System.Linq;

namespace HSPackParser.src.Data.CardInfos
{
    public static class CardInfoGetter
    {
        public static void GetCardInfo(List<Pack> packs)
        {
            packs.ForEach((pack) =>
            {
                pack.Cards.AsParallel().ForAll((card) =>
                {
                    if ((card != null) && (HearthDb.Cards.All.ContainsKey(card.Id)))
                    {
                        card.CardInfo = HearthDb.Cards.All[card.Id];
                    }
                });
            });
        }
    }
}
