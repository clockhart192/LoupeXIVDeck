namespace Loupedeck.LoupeXIVDeck.messages.outbound
{
    using System;
    using Newtonsoft.Json;
    public class InitOpCode
    {
        public String Opcode { get; private set; } = "init";
        public String Version { get; set; }
        [JsonProperty("mode")]
        public String Mode { get; set; }
    }
}
