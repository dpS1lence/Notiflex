using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notiflex.Core.Models.APIModels
{
    public class NameToCoordinatesModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("local_names")]
        public LocalNames LocalNames { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
    public class LocalNames
    {
        [JsonProperty("et")]
        public string Et { get; set; }

        [JsonProperty("pl")]
        public string Pl { get; set; }

        [JsonProperty("fi")]
        public string Fi { get; set; }

        [JsonProperty("de")]
        public string De { get; set; }

        [JsonProperty("hu")]
        public string Hu { get; set; }

        [JsonProperty("feature_name")]
        public string FeatureName { get; set; }

        [JsonProperty("fr")]
        public string Fr { get; set; }

        [JsonProperty("ro")]
        public string Ro { get; set; }

        [JsonProperty("ja")]
        public string Ja { get; set; }

        [JsonProperty("la")]
        public string La { get; set; }

        [JsonProperty("ascii")]
        public string Ascii { get; set; }

        [JsonProperty("bg")]
        public string Bg { get; set; }

        [JsonProperty("ru")]
        public string Ru { get; set; }

        [JsonProperty("lt")]
        public string Lt { get; set; }

        [JsonProperty("sk")]
        public string Sk { get; set; }

        [JsonProperty("uk")]
        public string Uk { get; set; }

        [JsonProperty("sr")]
        public string Sr { get; set; }

        [JsonProperty("ku")]
        public string Ku { get; set; }

        [JsonProperty("ar")]
        public string Ar { get; set; }

        [JsonProperty("el")]
        public string El { get; set; }

        [JsonProperty("tr")]
        public string Tr { get; set; }

        [JsonProperty("zh")]
        public string Zh { get; set; }

        [JsonProperty("nl")]
        public string Nl { get; set; }

        [JsonProperty("en")]
        public string En { get; set; }

        [JsonProperty("eu")]
        public string Eu { get; set; }

        [JsonProperty("hr")]
        public string Hr { get; set; }
    }
}
