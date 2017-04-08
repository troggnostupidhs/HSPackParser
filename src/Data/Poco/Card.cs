namespace HSPackParser.Data.Poco
{
    /// <summary>
    /// Represents the cards in a pack.
    /// </summary>
    public class Card
    {
        public string Id { get; set; }
        public bool IsGolden { get; set; }
        public HearthDb.Card CardInfo { get; set; }

        public Card()
        {
        }
    }
}