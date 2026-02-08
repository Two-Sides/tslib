using System;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Managers
{
    public class UtilityInstaller : MonoBehaviour
    {
        [SerializeField] protected IUtility[] Utilities;

        public virtual void RegisterUtilities(UtilityCtx utilityCtx)
        {
            if (utilityCtx == null) throw new ArgumentNullException(nameof(utilityCtx));

            if (Utilities == null) throw new InvalidOperationException(
                "(missing) utilities storage uninitialized.");

            for (int i = 0; i < Utilities.Length; i++)
            {
                utilityCtx.Register(Utilities[i]);
            }
            Utilities = null;
            Destroy(gameObject);
        }
    }
}
