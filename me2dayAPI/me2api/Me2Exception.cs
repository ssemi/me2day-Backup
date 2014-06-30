using System;
using System.Collections.Generic;
using System.Text;

namespace me2day.api
{
    public class Me2Exception : Exception
    {
        /// <summary>
        /// me2DAY Error
        /// </summary>
        public Me2Error Error { get; set; }

        public Me2Exception(Me2Error error)
        {
            Error = error;
        }
    }
}
