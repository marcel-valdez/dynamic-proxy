﻿// ***********************************************************************
// Assembly         : Test.AutoProxy
// Author           : Marcel Valdez
// Created          : 09-08-2012
//
// Last Modified By : Marcel Valdez
// Last Modified On : 09-08-2012
// ***********************************************************************
// <copyright file="MoreProperties.cs" company="Marcel Valdez">
//     Marcel Valdez. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Test.AutoProxy.HelperClasses
{

    /// <summary>
    /// This is a test class to verify the property mappings
    /// </summary>
    public class MoreProperties : Properties
    {
        /// <summary>
        /// Gets or sets the unmapped property.
        /// </summary>
        /// <value>The unmapped property.</value>
        public string UnmappedProperty
        {
            get;
            set;
        }
    }
}
