﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLite.Enumerations
{
    public enum CompareResult
    {
        /// <summary>
        /// Item do not appears
        /// </summary>
        Hidden,
        /// <summary>
        /// Item appears
        /// </summary>
        Visible,
        /// <summary>
        /// Item appears and will selected
        /// </summary>
        VisibleAndSelected
    }
}
