namespace Play.Catalog.Service.Settings
{
    public class RabbitMQSettings
    {
        public string Host { get; init; } //init to set because nobody should be setting these properties after they have been deserialized from the configuration file.
    }
}