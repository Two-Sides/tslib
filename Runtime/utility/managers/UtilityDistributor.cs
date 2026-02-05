using System;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Managers
{
    public class UtilityDistributor : MonoBehaviour
    {
        [SerializeField] protected IUtility[] Utilities;

        public virtual void RegisterUtilities(AppCtx appCtx)
        {
            if (appCtx == null) throw new ArgumentNullException(nameof(appCtx));

            if (Utilities == null) throw new InvalidOperationException(
                "(missing) utilities storage uninitialized.");

            for (int i = 0; i < Utilities.Length; i++)
            {
                appCtx.UtilityCtx.Register(Utilities[i]);
            }
        }
    }
}
