
//namespace Hospital.Services.ClinicAPI.Services
//{
//    using Newtonsoft.Json;
//    using System;
//    using System.Text.Json;
//    using System.Text.Json.Serialization;

//    public class TimeOnlyConverter : JsonConverter<TimeSpan>
//    {
//        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
//        {
//            // Deserialize the TimeSpan from JSON string
//            var timeString = (string)reader.Value;
//            return TimeSpan.Parse(timeString);
//        }

//        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
//        {
//            // Serialize the TimeSpan to JSON string
//            writer.WriteValue(value.ToString(@"hh\:mm\:ss"));
//        }
//    }

//}