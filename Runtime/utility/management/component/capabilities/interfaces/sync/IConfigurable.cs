namespace TSLib.Utility.Management.Component.Capabilities
{
    public interface IConfigurable
    {
        /// <summary>
        /// Configures the object.
        /// </summary>
        public void Configure();

        /// <summary>
        /// Deconfigures the object.
        /// </summary>
        public void Deconfigure();
    }
}

