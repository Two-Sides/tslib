using System.Threading;
using Cysharp.Threading.Tasks;

namespace TSLib.Utility.Management.Component.Capabilities.Async
{
    public interface IInitializableAsync
    {
        /// <summary>
        /// Initializes the component.
        /// </summary>
        public UniTask Initialize(CancellationToken cancellationToken);
    }
}