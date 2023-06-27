using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CQRS.Core.Events;
using Post.Comon.Events;

namespace Post.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }
        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out JsonDocument? doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
            }

            if (!doc.RootElement.TryGetProperty("Type", out JsonElement type))
            {
                throw new JsonException("Could not detect the Type discriminator property");
            }

            string typeDiscriminator = type.GetString();
            string json = doc.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                nameof(PublicacaoCriadaEvent) => JsonSerializer.Deserialize<PublicacaoCriadaEvent>(json, options),
                nameof(MensagemEditadaEvent) => JsonSerializer.Deserialize<MensagemEditadaEvent>(json, options),
                nameof(PublicacaoCurtidaEvent) => JsonSerializer.Deserialize<PublicacaoCurtidaEvent>(json, options),
                nameof(ComentarioAdicionadoEvent) => JsonSerializer.Deserialize<ComentarioAdicionadoEvent>(json, options),
                nameof(ComentarioEditadoEvent) => JsonSerializer.Deserialize<ComentarioEditadoEvent>(json, options),
                nameof(ComentarioRemovidoEvent) => JsonSerializer.Deserialize<ComentarioRemovidoEvent>(json, options),
                nameof(PublicacaoExcluidaEvent) => JsonSerializer.Deserialize<PublicacaoExcluidaEvent>(json, options),
                _ => throw new JsonException($"{typeDiscriminator} is not supported yet")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}