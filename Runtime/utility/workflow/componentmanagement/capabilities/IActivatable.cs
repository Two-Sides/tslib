using Cysharp.Threading.Tasks;
using System.Threading;

namespace TwoSides.Utility.Workflow.ComponentManagement.Capabilities
{
    public interface IActivatable
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

