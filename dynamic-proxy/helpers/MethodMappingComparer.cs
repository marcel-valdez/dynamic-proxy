//-----------------------------------------------------------------------
// <copyright file="MethodMappingComparer.cs" company="Route Manager de México">
//     Copyright Route Manager de México, 2011(c). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DynamicProxy
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonUtilities;

    /// <summary>
    /// Responsabilidad: Comparar dos objetos tipo <typeparamref name="IMethodMapping"/>
    /// Encapsula: La implementación para comparar dos objetos <typeparamref name="IMethodMapping"/>
    /// </summary>
    public class MethodMappingComparer : IEqualityComparer<IMethodMapping>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T"/> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(IMethodMapping x, IMethodMapping y)
        {
            if(x == null || y == null)
            {
                return false;
            }

            int xArgumentTypesCount = x.ArgumentTypes.Count();
            int yArgumentTypesCount = y.ArgumentTypes.Count();
            int xGenericArgumentTypesCount = x.GenericArgumentTypes.Count();
            int yGenericArgumentTypesCount = y.GenericArgumentTypes.Count();

            if(!(x.Name == y.Name && xArgumentTypesCount == yArgumentTypesCount && xGenericArgumentTypesCount == yGenericArgumentTypesCount))
            {
                return false;
            }

            return x.ArgumentTypes.Intersect(y.ArgumentTypes).Count() == xArgumentTypesCount 
                && x.GenericArgumentTypes.Intersect(y.GenericArgumentTypes).Count() == xGenericArgumentTypesCount;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        ///   </exception>
        public int GetHashCode(IMethodMapping obj)
        {
            object[] array = obj != null ? new object[] { obj.Name, obj.ArgumentTypes, obj.GenericArgumentTypes }
                                         : new object[] { "", System.Type.EmptyTypes, System.Type.EmptyTypes };
            return array.GetArrayHashCode();
        }
    }
}
