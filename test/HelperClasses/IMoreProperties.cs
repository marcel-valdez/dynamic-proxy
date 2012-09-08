// -----------------------------------------------------------------------
// <copyright file="IMoreProperties.cs" company="">
// Marcel Valdez, Copyright 2012 (r).
// </copyright>
// -----------------------------------------------------------------------
namespace Test.AutoProxy.HelperClasses
{

    /// <summary>
    /// Has one extra property besides the IProperties
    /// </summary>
    public interface IMoreProperties : IProperties
    {
        /// <summary>
        /// Gets a property that can't be found in IPropertires nor Properties
        /// </summary>
        int ExtraGetProperty
        {
            get;
        }

        /// <summary>
        /// Gets or sets a property that can't be found in IPropertires nor Properties
        /// </summary>
        /// <value>The extra property with getter and setter.</value>
        int ExtraGetSetProperty
        {
            get;
            set;
        }
    }
}
