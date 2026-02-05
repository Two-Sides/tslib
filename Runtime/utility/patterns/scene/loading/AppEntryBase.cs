using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Debug.Logging;
using TSLib.Utility.Patterns.EventChannels.NonPrimitive;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Patterns.Scene.Loading
{
    public abstract class AppEntryBase : MonoBehaviour
    {
        [SerializeField] private SceneChannel_So onAppLoaded;

        public AppCtx Context { get; protected set; }

        private async void Start()
        {
            var ct = this.GetCancellationTokenOnDestroy();

            try
            {
                await ConfigureAppAsync(ct);
                Context = await CreateContextAsync(ct);
                await RegisterUtilitiesCtxAsync(ct);
                await RegisterSharedCtxAsync(ct);
                await LoadFirstSceneAdditiveAsync(ct);

                if (ct.IsCancellationRequested)
                    return;

                var scene = gameObject.scene;
                if (!scene.IsValid() || !scene.isLoaded)
                    throw new InvalidOperationException("(not loaded) scene was not loaded");

                if (onAppLoaded != null)
                    onAppLoaded.TriggerEvent(scene);
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
                // persistent, it acts as the container of app context.
                DontDestroyOnLoad(gameObject);
            }
        }

        protected abstract UniTask ConfigureAppAsync(CancellationToken ct);
        protected abstract UniTask<AppCtx> CreateContextAsync(CancellationToken ct);
        protected virtual UniTask RegisterUtilitiesCtxAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask RegisterSharedCtxAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected abstract UniTask LoadFirstSceneAdditiveAsync(CancellationToken ct);
        protected virtual void OnTokenCanceled() { }
    }
}
