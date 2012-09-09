using System;

namespace Test.AutoProxy.HelperClasses
{
    public interface IExtraTestInterface<T> : ITestInterface<T>
    {
        string ExtraMethod(int parameter);

        void ExtraVoidMethod(int parameter);

        string ExtraStringMethodNoParams();

        void ExtraVoidMethodNoParams();
    }

    public interface ITestInterface<T>
    {
        string GetAndSetProperty
        {
            get;
            set;
        }

        int GetOnlyProperty
        {
            get;
        }

        int IntParameterLess();

        T GenericParameterLess();

        int IntOverloaded(string parameter);

        int IntOverloaded();

        int IntOverloaded(T param);

        int IntOverloaded<TParam>(TParam param);

        void VoidOverloaded();

        void VoidOverloaded(string parameter);

        void VoidOverloaded(T param);

    }
}
