using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Patterns.EventChannels.NonPrimitive;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Patterns.Scene.Loading
{
    public abstract class SceneEntryBase : MonoBehaviour
    {
        [SerializeField] private SceneChannel_So onSceneLoaded;

        protected AppCtx AppCtx;
        protected SceneCtx SceneCtx;

        public async UniTask LoadAsync(AppCtx appCtx, CancellationToken ct)
        {
            AppCtx = appCtx ?? throw new ArgumentNullException(nameof(appCtx));
            SceneCtx = new SceneCtx();

            SceneCtx.SetActive(false);

            await PreconfigureSceneAsync(ct);
            await InstantiateAsync(ct);
            await InitializeAsync(ct);
            await RegisterAsync(SceneCtx, AppCtx, ct);

            SceneCtx.SetActive(true);

            await BindingAsync(SceneCtx, AppCtx, ct);
            await ConfigureAsync(ct);

            // optional
            await ExecuteCustomOperationsAsync(ct);
            await LoadSceneAdditiveAsync(ct);
            await UnLoadSceneAdditiveAsync(ct);

            if (onSceneLoaded == null) return;
            onSceneLoaded.TriggerEvent(gameObject.scene);
        }

        protected virtual UniTask PreconfigureSceneAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected abstract UniTask InstantiateAsync(CancellationToken ct);
        protected abstract UniTask InitializeAsync(CancellationToken ct);
        protected abstract UniTask RegisterAsync(SceneCtx sceneCtx, AppCtx appCtx, CancellationToken ct);
        protected abstract UniTask BindingAsync(SceneCtx sceneCtx, AppCtx appCtx, CancellationToken ct);
        protected abstract UniTask ConfigureAsync(CancellationToken ct);
        protected virtual UniTask ExecuteCustomOperationsAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask LoadSceneAdditiveAsync(CancellationToken ct) => UniTask.CompletedTask;
        protected virtual UniTask UnLoadSceneAdditiveAsync(CancellationToken ct) => UniTask.CompletedTask;
    }
}

