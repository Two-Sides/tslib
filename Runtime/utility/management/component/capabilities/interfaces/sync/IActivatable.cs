namespace TSLib.Utility.Management.Component.Capabilities
{
    public interface IActivatable
    {
        /// <summary>
        /// Activates the element.
        /// </summary>
        public void Activate();

        /// <summary>
        /// Deactivates the element.
        /// </summary>
        public void DeactivateAsync();
    }
}
