// ***********************************************************************
// Assembly         : AutoProxy
// Author           : Marcel Valdez
// Created          : 09-08-2012
//
// Last Modified By : Marcel Valdez
// Last Modified On : 09-08-2012
// ***********************************************************************
// <copyright file="With.cs" company="Marcel Valdez">
//     Marcel Valdez. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AutoProxy.Fluent
{

    /// <summary>
    /// Class with argument type definitions
    /// </summary>
    public static class With
    {
        /// <summary>
        /// Gets a marker for the <typeparamref name="T"/> type
        /// </summary>
        /// <typeparam name="T">The argument type</typeparam>
        /// <returns>A marker</returns>
        public static T Arg<T>()
        {
         	return default(T);
        }
    }
}
