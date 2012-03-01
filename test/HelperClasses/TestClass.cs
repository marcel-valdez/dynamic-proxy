namespace Test.AutoProxy.HelperClasses
{
    class TestClass : ITestInterface<double>
    {
        public static int IntParameterLessReturn = 0;
        public static double GenericParameterLessReturn = 1;
        public static int IntOverloadedStringReturn = 1;
        public static int IntOverloadedNoParamReturn = 2;
        public static int IntOverloadedGenericReturn = 3;
        public static int IntOverloadedDoubleReturn = 4;
        public bool VoidOverloadedCalled = false;
        public bool VoidOverloadedStringCalled = false;
        public bool VoidOverloadedGenericCalled = false;
        private readonly int getOnlyProperty;

        public TestClass(string defaultGetSet = "getset", int defaultGet = 10)
        {
            this.GetAndSetProperty = defaultGetSet;
            this.getOnlyProperty = defaultGet;
        }

        public TestClass()
        {
            this.GetAndSetProperty = "getset";
            this.getOnlyProperty = 10;
        }

        public string GetAndSetProperty
        {
            get;
            set;
        }

        public int GetOnlyProperty
        {
            get
            {
                return getOnlyProperty;
            }
        }

        public int IntParameterLess()
        {
            return IntParameterLessReturn;
        }

        public double GenericParameterLess()
        {
            return GenericParameterLessReturn;
        }

        public int IntOverloaded(string parameter)
        {
            return IntOverloadedStringReturn;
        }

        public int IntOverloaded()
        {
            return IntOverloadedNoParamReturn;
        }

        public int IntOverloaded(double param)
        {
            return IntOverloadedDoubleReturn;
        }

        public int IntOverloaded<T>(T param)
        {
            return IntOverloadedGenericReturn;
        }

        public void VoidOverloaded()
        {
            this.VoidOverloadedCalled = true;
        }

        public void VoidOverloaded(string parameter)
        {
            this.VoidOverloadedStringCalled = true;
        }

        public void VoidOverloaded(double param)
        {
            this.VoidOverloadedGenericCalled = true;
        }
    }
}