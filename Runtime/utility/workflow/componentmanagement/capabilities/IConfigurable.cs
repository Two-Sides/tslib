namespace TwoSides.Utility.Workflow.ComponentManagement.Capabilities
{
    public interface IConfigurable
    {
        /// <summary>
        /// Configures the object's values.
        /// </summary>
        public void Configure();

        /// <summary>
        /// Deconfigures the object so it can be destroyed.
        /// </summary>
        public void Deconfigure();
    }
}

