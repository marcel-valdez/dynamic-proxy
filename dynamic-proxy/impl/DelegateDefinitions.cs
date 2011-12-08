namespace DynamicProxy
{
    /// <summary>
    /// Generic mapped method
    /// </summary>
    /// <typeparam name="T">Tipo de objeto del resultado</typeparam>
    /// <param name="args">Arguments passed in to the method</param>
    /// <returns>Instancia tipo <typeparamref name="T"/></returns>
    public delegate T MappedMethod<T>(params object[] args);

    /// <summary>
    /// Concrete mapped method
    /// </summary>
    /// <param name="args">Arguments passed in to the method</param>
    /// <returns>object result</returns>
    public delegate object MappedMethod(params object[] args);
}
