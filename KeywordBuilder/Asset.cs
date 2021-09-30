using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordBuilder
{
    class Asset
    {
        [JsonProperty("asset_id")]
        public string AssetID { get; set; }
        public string URL { get; set; }
        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("keywords_detected")]
        public bool KeywordsDetected { get; set; }

        public bool UpdatedKeywords { get; set; }

        [JsonProperty("components")]
        public List<Asset> Components { get; set; }

    }
}
