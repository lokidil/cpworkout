using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CPWorkout
{
    public class Program
    {
        [JsonProperty(PropertyName = "id")]
        public ProgramId Id { get; set; }

        public Program(string id)
        {
            Id = ProgramId.FromString(id);
        }
        public required string Title { get; set; }
        public required string Description { get; set; }
        // Country need to be used as partition key
        public string Country { get; set; } = "India";
        public Question[]? Questions { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Question
    {
        [JsonProperty(PropertyName = "id")]
        public QuestionId Id { get; set; }

        public Question(string id)
        {
            Id = QuestionId.FromString(id);
        }
        public required string Title { get; set; } = "";
        public required string QuestionType { get; set; } = "";
        public string[]? Options { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [JsonConverter(typeof(ProgramIdConverter))]
    public class ProgramId
    {
        public string Value { get; internal set; }
        protected ProgramId() { }

        internal ProgramId(string value) => Value = value;

        public static ProgramId FromString(string programId)
        {
            CheckValidity(programId);
            return new ProgramId(programId);
        }

        public static implicit operator string(ProgramId programId) => programId.Value;

        public override string ToString()
        {
            return Value;
        }
        private static void CheckValidity(string value)
        {
            if (!Guid.TryParse(value, out _))
            {
                throw new ArgumentException(nameof(value), "Program Id is not a GUID.");
            }
        }
    }

    class ProgramIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ProgramId));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ProgramId.FromString(JToken.Load(reader).ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken.FromObject(value.ToString()).WriteTo(writer);
        }
    }

    [JsonConverter(typeof(QuestionIdConverter))]
    public class QuestionId
    {
        public string Value { get; internal set; }
        protected QuestionId() { }

        internal QuestionId(string value) => Value = value;

        public static QuestionId FromString(string questionId)
        {
            CheckValidity(questionId);
            return new QuestionId(questionId);
        }

        public static implicit operator string(QuestionId questionId) => questionId.Value;

        public override string ToString()
        {
            return Value;
        }
        private static void CheckValidity(string value)
        {
            if (!Guid.TryParse(value, out _))
            {
                throw new ArgumentException(nameof(value), "Question Id is not a GUID.");
            }
        }
    }

    class QuestionIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(QuestionId));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return QuestionId.FromString(JToken.Load(reader).ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken.FromObject(value.ToString()).WriteTo(writer);
        }
    }

    public class Candidate
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }
        public required string ProgramId { get; set; }
        public Application[]? Applications { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Application
    {
        public required string QuestionId { get; set; }
        public string[]? Responses { get; set; }
    }
}
