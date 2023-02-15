using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notiflex.Core.Models.ForecastApiModel.DailyModel;

namespace Notiflex.Core.Models.ForecastApiModel
{
    public class ForecastDataModel
    {
        [JsonProperty("cod")]
        public string Cod { get; set; } = null!;

        [JsonProperty("message")]
        public int Message { get; set; }

        [JsonProperty("cnt")]
        public int Cnt { get; set; }

        [JsonProperty("list")]
        public List<DailyDataModel> List { get; set; } = null!;

        [JsonProperty("city")]
        public City City { get; set; } = null!;
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("coord")]
        public Coord Coord { get; set; } = null!;

        [JsonProperty("country")]
        public string Country { get; set; } = null!;

        [JsonProperty("population")]
        public int Population { get; set; }

        [JsonProperty("timezone")]
        public int Timezone { get; set; }

        [JsonProperty("sunrise")]
        public int Sunrise { get; set; }

        [JsonProperty("sunset")]
        public int Sunset { get; set; }
    }

    public class Clouds
    {
        [JsonProperty("all")]
        public int All { get; set; }
    }

    public class Coord
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }

    public class List
    {
        [JsonProperty("dt")]
        public int Dt { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; } = null!;

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; } = null!;

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; } = null!;

        [JsonProperty("wind")]
        public Wind Wind { get; set; } = null!;

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("pop")]
        public double Pop { get; set; }

        [JsonProperty("sys")]
        public Sys Sys { get; set; } = null!;

        [JsonProperty("dt_txt")]
        public string DtTxt { get; set; } = null!;

        [JsonProperty("rain")]
        public Rain Rain { get; set; } = null!;

        [JsonProperty("snow")]
        public Snow Snow { get; set; } = null!;
    } 

    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("sea_level")]
        public int SeaLevel { get; set; }

        [JsonProperty("grnd_level")]
        public int GrndLevel { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("temp_kf")]
        public double TempKf { get; set; }
    }

    public class Rain
    {
        [JsonProperty("3h")]
        public double _3h { get; set; }
    }

    public class Snow
    {
        [JsonProperty("3h")]
        public double _3h { get; set; }
    }

    public class Sys
    {
        [JsonProperty("pod")]
        public string Pod { get; set; }
    }

    public class Weather
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("icon")]
        public string Icon { get; set; } = null!;
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public int Deg { get; set; }

        [JsonProperty("gust")]
        public double Gust { get; set; }
    }


}
