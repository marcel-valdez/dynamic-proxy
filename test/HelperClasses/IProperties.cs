namespace Test.AutoProxy.HelperClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IProperties
    {
        string Name
        {
            get;
            set;
        }

        int Age
        {
            get;
            set;
        }

        object Data
        {
            get;
            set;
        }
    }
}
