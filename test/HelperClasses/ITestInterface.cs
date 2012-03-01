using System;

namespace Test.AutoProxy.HelperClasses
{
    interface ITestInterface<T>
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
