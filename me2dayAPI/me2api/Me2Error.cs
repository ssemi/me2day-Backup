﻿using System;
using System.Collections.Generic;
using System.Text;

namespace me2day.api
{
    public class Me2Error
    {
        /// <summary>
        /// me2DAY Defined Error code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 에러 메세지
        /// </summary>
        public String Message { get; set; }
        /// <summary>
        /// 에러 설명
        /// </summary>
        public String Description { get; set; }
    }
}
