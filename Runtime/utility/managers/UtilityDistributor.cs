using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Managers
{
    public class UtilityDistributor : MonoBehaviour
    {
        [SerializeField] protected IUtility[] Utilities;

        public virtual void RegisterUtilities(AppCtx appCtx)
        {
            for (int i = 0; i < Utilities.Length; i++)
            {
                var utility = Utilities[i];
                appCtx.UtilityCtx.Register(utility);
            }
        }
    }
}
