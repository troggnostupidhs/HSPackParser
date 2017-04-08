using System.Collections.Generic;

namespace HSPackParser.Data.Poco
{
    /// <summary>
    /// Represents a pack.
    /// </summary>
    public class Pack
    {
        public string Id { get; set; }
        public string BoosterType { get; set; }
        public string OpenDate { get; set; }
        public string UploaderId { get; set; }
        public string AccountId { get; set; }
        public string RegionId { get; set; }
        public List<Card> Cards { get; set; }

        public Pack()
        {
        }
    }
}