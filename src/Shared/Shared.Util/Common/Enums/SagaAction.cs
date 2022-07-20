using System.ComponentModel;

namespace Shared.Util.Common.Enums
{

    /// <summary>
    /// Enumerator that represent actions on SAGA Pattern.
    /// </summary>
    public enum SagaAction
    {
        /// <summary>
        /// Represent a action Confirm. Name **Confirm**, description **Confirm** and code **1**.
        /// </summary>
        [Description("Confirm")] Confirm = 1,
        /// <summary>
        /// Represent a action Revert. Name **Revert**, description **Revert** and code **2**.
        /// </summary>
        [Description("Revert")] Revert = 2,
    }
}
