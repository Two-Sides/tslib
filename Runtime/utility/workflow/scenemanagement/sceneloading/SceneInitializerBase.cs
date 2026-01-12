using System.Threading;
using Cysharp.Threading.Tasks;

namespace TwoSides.Utility.Workflow.SceneManagement.SceneLoading
{
    /// <summary>
    /// Base class that defines a structured, multi-phase workflow for scene initialization.
    ///
    /// This class extends <see cref="SingleEntryPointBase"/> and provides a fixed execution
    /// pipeline composed of well-defined steps:
    ///
    /// 1. Instantiation of required objects and resources.
    /// 2. Registration of services in <see cref="ServiceLocator"/> (you should resolve dependencies here).
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
    public abstract class SceneInitializerBase : SingleEntryPointBase
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
            await InstantiateAsync(ct);
            await RegisterAsync(ct);
            await InitializeAsync(ct);
            await Configure(ct);
            await ExecuteCustomOperationsAsync(ct);
        }

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

        /// Executes configuration logic after all instantiation, registration,
        /// and initialization steps have completed.
        ///
        /// This phase is intended for setup work that depends on fully initialized
        /// systems and may require asynchronous operations.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected abstract UniTask Configure(CancellationToken ct);

        /// <summary>
        /// Executes optional custom operations that extend or finalize the
        /// initialization workflow.
        ///
        /// This phase is intended for logic that does not naturally belong to
        /// instantiation, registration, initialization, or configuration, but
        /// must run once all previous steps have completed.
        ///
        /// By default, this method delegates execution to the synchronous
        /// <see cref="ExecuteCustomOperations"/> implementation and returns a
        /// completed task.
        ///
        /// Override this method to implement asynchronous continuation logic.
        /// Alternatively, override <see cref="ExecuteCustomOperations"/> to
        /// implement synchronous behavior when no awaiting is required.
        /// </summary>
        /// <example>
        /// If all GameObjects were instantiated in an inactive state (recommended),
        /// they can be activated sequentially by overriding
        /// <see cref="ExecuteCustomOperations"/> or asynchronously by overriding
        /// this method.
        /// </example>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected virtual UniTask ExecuteCustomOperationsAsync(CancellationToken ct)
        {
            ExecuteCustomOperations(ct);
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Executes optional synchronous custom operations at the end of the
        /// initialization workflow.
        ///
        /// This method is intended for immediate, non-blocking logic that does
        /// not require awaiting asynchronous operations.
        ///
        /// Override this method when custom behavior can be expressed
        /// synchronously. For asynchronous behavior, override
        /// <see cref="ExecuteCustomOperationsAsync"/> instead.
        ///
        /// Implementations should observe the provided <see cref="CancellationToken"/> 
        /// to avoid performing work after cancellation has been requested.
        /// </summary>
        /// <example>
        /// You can handle cancellation like this:
        /// <code>
        /// ct.ThrowIfCancellationRequested();
        /// // or
        /// if (ct.IsCancellationRequested) return;
        /// </code>
        /// </example>
        /// <param name="ct">
        /// Cancellation token that is triggered when the owning GameObject is destroyed.
        /// </param>
        protected virtual void ExecuteCustomOperations(CancellationToken ct) { }
    }
}

