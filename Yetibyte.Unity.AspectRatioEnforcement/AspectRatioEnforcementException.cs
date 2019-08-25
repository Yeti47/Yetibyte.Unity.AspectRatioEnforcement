using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Unity.AspectRatioEnforcement {
    public class AspectRatioEnforcementException : Exception {

        #region Constants

        private const string DEFAULT_MSG = "Error setting the target aspect ratio.";

        #endregion

        #region Ctors

        public AspectRatioEnforcementException(string message, Exception innerException) : base(message, innerException) {

        }

        public AspectRatioEnforcementException(string message = DEFAULT_MSG) : base(message) {

        }

        #endregion

    }
}
