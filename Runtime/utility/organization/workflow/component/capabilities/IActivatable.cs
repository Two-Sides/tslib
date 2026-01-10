using Cysharp.Threading.Tasks;
using System.Threading;

public interface IActivatable
{
    /// <summary>
    /// Activates the element.
    /// </summary>
    UniTask ActivateAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Deactivates the element.
    /// </summary>
    UniTask DeactivateAsync(CancellationToken cancellationToken);
}