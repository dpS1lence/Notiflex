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
        public string Name { get; set; } = null!;

        [JsonProperty("local_names")]
        public LocalNames LocalNames { get; set; } = null!;

        [JsonProperty("lat")]
        public double Lat { get; set; } 

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; } = null!;
    }
    public class LocalNames
    {
        [JsonProperty("et")]
        public string Et { get; set; } = null!;

        [JsonProperty("pl")]
        public string Pl { get; set; } = null!;

        [JsonProperty("fi")]
        public string Fi { get; set; } = null!;

        [JsonProperty("de")]
        public string De { get; set; } = null!;

        [JsonProperty("hu")]
        public string Hu { get; set; } = null!;

        [JsonProperty("feature_name")]
        public string FeatureName { get; set; } = null!;

        [JsonProperty("fr")]
        public string Fr { get; set; } = null!;

        [JsonProperty("ro")]
        public string Ro { get; set; } = null!;

        [JsonProperty("ja")]
        public string Ja { get; set; } = null!;

        [JsonProperty("la")]
        public string La { get; set; } = null!;

        [JsonProperty("ascii")]
        public string Ascii { get; set; } = null!;

        [JsonProperty("bg")]
        public string Bg { get; set; } = null!;

        [JsonProperty("ru")]
        public string Ru { get; set; } = null!;

        [JsonProperty("lt")]
        public string Lt { get; set; } = null!;

        [JsonProperty("sk")]
        public string Sk { get; set; } = null!;

        [JsonProperty("uk")]
        public string Uk { get; set; } = null!;

        [JsonProperty("sr")]
        public string Sr { get; set; } = null!;

        [JsonProperty("ku")]
        public string Ku { get; set; } = null!;

        [JsonProperty("ar")]
        public string Ar { get; set; } = null!;

        [JsonProperty("el")]
        public string El { get; set; } = null!;

        [JsonProperty("tr")]
        public string Tr { get; set; } = null!;

        [JsonProperty("zh")]
        public string Zh { get; set; } = null!;

        [JsonProperty("nl")]
        public string Nl { get; set; } = null!;

        [JsonProperty("en")]
        public string En { get; set; } = null!;

        [JsonProperty("eu")]
        public string Eu { get; set; } = null!;

        [JsonProperty("hr")]
        public string Hr { get; set; } = null!;
    }
}
