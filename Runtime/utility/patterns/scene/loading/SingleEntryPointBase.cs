using Cysharp.Threading.Tasks;
using UnityEngine;
using TwoSides.Utility.Patterns.EventChannels.NonPrimitive;
using System;
using System.Threading;
using TwoSides.Utility.Debug.Logging;

namespace TwoSides.Utility.Patterns.Scene.Loading
{
    /// <summary>
    /// Base class that acts as a single entry point for scene initialization logic.
    /// 
    /// Implementations should perform all required asynchronous initialization work
    /// inside <see cref="Execute"/>, which is automatically invoked when the scene starts.
    /// 
    /// Once initialization completes successfully, this component notifies listeners
    /// by raising a scene-initialized event and then destroys its own GameObject.
    /// 
    /// Cancellation is automatically handled when the GameObject is destroyed,
    /// preventing notifications from being sent in invalid or unloaded scenes.
    /// </summary>
    public abstract class SingleEntryPointBase : MonoBehaviour
    {
        /// <summary>
        /// Event channel triggered when the scene has finished its initialization.
        /// </summary>
        [SerializeField] private SceneChannelSo onSceneInitialized;

        /// <summary>
        /// Executes the scene-specific asynchronous initialization logic.
        /// Implementations should observe the provided cancellation token and
        /// stop execution when cancellation is requested.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token that is triggered when this GameObject is destroyed.
        /// </param>
        protected abstract UniTask Execute(CancellationToken ct);

        private async void Start()
        {
            var ct = this.GetCancellationTokenOnDestroy();

            try
            {
                await Execute(ct);

                if (ct.IsCancellationRequested)
                    return;

                var scene = gameObject.scene;
                if (!scene.IsValid() || !scene.isLoaded) return;

                onSceneInitialized.TriggerEvent(scene);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                OnTokenCanceled(); // could be ignored
            }
            catch (Exception ex)
            {
                TSLogger.LogException(ex, this);
            }
            finally
            {
                // Self-destruct after initialization completes or fails.
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Called when the initialization workflow is canceled due to this
        /// component's destruction (i.e., when the <see cref="CancellationToken"/>
        /// obtained from <c>GetCancellationTokenOnDestroy()</c> is canceled).
        ///
        /// This method is invoked only when <see cref="Execute"/> observes the token
        /// and throws an <see cref="OperationCanceledException"/> as a result of that
        /// cancellation.
        ///
        /// Override this to perform optional cleanup or diagnostics (e.g., logging,
        /// resetting transient state). Implementations should be fast, side-effect
        /// safe, and must not assume the scene is still loaded.
        /// </summary>
        protected virtual void OnTokenCanceled() { }
    }
}



