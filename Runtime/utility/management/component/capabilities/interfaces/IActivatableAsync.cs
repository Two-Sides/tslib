using Cysharp.Threading.Tasks;
using System.Threading;

namespace TwoSides.Utility.Management.Component.Capabilities
{
    public interface IActivatableAsync
    {
        /// <summary>
        /// Activates the element.
        /// </summary>
        public UniTask ActivateAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Deactivates the element.
        /// </summary>
        public UniTask DeactivateAsync(CancellationToken cancellationToken);
    }
}

