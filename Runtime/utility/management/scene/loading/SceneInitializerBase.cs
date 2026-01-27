using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Management.Service;
using TSLib.Utility.Patterns.Scene.Loading;

namespace TSLib.Utility.Management.Scene.Loading
{
    /// <summary>
    /// Base class that defines a structured, multi-phase workflow for scene initialization.
    ///
    /// This class extends <see cref="SceneEntryBase"/> and provides a fixed execution
    /// pipeline composed of well-defined steps:
    ///
    /// 1. Instantiation of required objects and resources.
    /// 2. Registration of services in <see cref="Service.ServiceLocator"/> (you should 
    ///     resolve dependencies here).
    /// 3. Initialization of elements.
    /// 4. Configuration of elements.
    /// 5. Optional custom continuation logic (synchronous or asynchronous).
    ///
    /// Derived classes implement the abstract steps to provide scene-specific loading behavior,
    /// while the overall execution order and lifecycle management remain centralized.
    ///
    /// All phases receive the same <see cref="CancellationToken"/>, allowing the workflow
    /// to cooperatively stop execution when the owning GameObject is destroyed.
    /// </summary>
    public abstract class SceneInitializerBase : SceneEntryBase
    {
        /// <summary>
        /// Executes the complete scene initialization workflow in a strict, sequential order.
        /// This method is called automatically by the base class.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected sealed override async UniTask Execute(CancellationToken ct)
        {
            ServiceLocator.SetActive(false);

            await PreconfigureEnvironmentAsync(ct);
            await InstantiateAsync(ct);
            await RegisterAsync(ct);
            await InitializeAsync(ct);

            ServiceLocator.SetActive(true);

            await BindingAsync(ct);
            await ConfigureAsync(ct);
            await ExecuteCustomOperationsAsync(ct);
        }

        /// <summary>
        /// TODO
        /// </summary>
        protected abstract UniTask PreconfigureEnvironmentAsync(CancellationToken ct);

        /// <summary>
        /// Instantiates all scene-specific objects, assets, or resources required
        /// for the initialization workflow.
        ///
        /// This is the earliest phase of the pipeline and should focus solely on
        /// creating or loading entities, without assuming that any systems or
        /// dependencies have been registered or initialized yet.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected abstract UniTask InstantiateAsync(CancellationToken ct);

        /// <summary>
        /// Registers previously instantiated systems, services, or components
        /// into the appropriate containers, service locators, or registries.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected abstract UniTask RegisterAsync(CancellationToken ct);

        /// <summary>
        /// Executes initialization logic for all previously registered elements.
        ///
        /// At this stage, all dependencies are expected to be fully instantiated
        /// and registered, making them safe to access and initialize.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected abstract UniTask InitializeAsync(CancellationToken ct);

        /// <summary>
        /// Executes the binding phase of the lifecycle, where dependencies are
        /// explicitly linked between already instantiated and initialized objects.
        ///
        /// During this stage, objects are expected to resolve and store references
        /// to other systems, services, or components they depend on. No new instances
        /// should be created here; the purpose of this phase is strictly to connect
        /// existing elements together.
        ///
        /// This method enables dependency injection at runtime, ensuring that all
        /// references are wired only after every required dependency is known to
        /// exist and be fully initialized.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected abstract UniTask BindingAsync(CancellationToken ct);

        /// <summary>
        /// Executes configuration logic after all instantiation, registration,
        /// and initialization steps have completed.
        ///
        /// This phase is intended for setup work that depends on fully initialized
        /// systems and may require asynchronous operations.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected abstract UniTask ConfigureAsync(CancellationToken ct);

        /// <summary>
        /// Executes optional custom operations that extend or finalize the
        /// scene initialization workflow.
        ///
        /// This phase is intended for logic that does not naturally belong to
        /// instantiation, registration, initialization, or configuration, but
        /// must run once all previous steps have completed.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected virtual UniTask ExecuteCustomOperationsAsync(CancellationToken ct) => UniTask.CompletedTask;
    }
}

