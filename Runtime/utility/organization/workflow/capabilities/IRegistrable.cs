namespace TwoSides.Utility.Organization.Workflow.Capabilities
{
    public interface IRegistrable
    {
        /// <summary>
        /// Registers the component with the
        /// relevant services.
        /// </summary>
        public void Register();

        /// <summary>
        /// Removes the component from the
        /// services where it was registered.
        /// </summary>
        public void Unregister();
    }
}

