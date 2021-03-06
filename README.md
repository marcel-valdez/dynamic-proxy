What is it?
===========
**Dynamic proxy (a.k.a. AutoProxy)** is an automatic proxy creator, meant to wrap classes of
which we don't have the source code, but need to make them testable, mockable,
or substitutable by wrapping them with an interface.

Sample Usage
============

This is the class we want to profixy, normally, this class is in
a third party library or framework, otherwise we would just add **`Wrapped : IWrapper`** to the class definition


`````csharp
public class Wrapped {
  private string mProperty = "";
  private string mUnmatched = "";
  public void CallMethod() {
      Console.WriteLine("Wrapped.CallMethod()");
  }

  public string Property {
      get {
        Console.WriteLine("Wrapped.Property.get, with value: {0}", this.mProperty);
        return this.mProperty;
      }
  
      set {
        Console.WriteLine("Wrapped.Property.set, with value: {0}", value);
        this.mProperty = value;
      }
  }

  public string UnmatchedProperty {
      get {
        Console.WriteLine("Wrapped.UnmatchedProperty.get, returned value: {0}", this.mUnmatched);
        return this.mUnmatched;
      }

      set {
        Console.WriteLine("Wrapped.UnmatchedProperty.set, input value: {0}", value);
        this.mUnmatched = value;
      }
  }
}
`````
The <b>`IWrapper`</b> interface through which we will use the **`Wrapped`** object
`````csharp
public interface IWrapper {
  void CallMethod();
  string Property { get; set; }  
  int NonMatch { get; set; }
}
`````

In order to "proxify" the **`Wrapped`** class we just use the following
  
`````csharp
void Main() {
    Wrapped wrapped = new Wrapped();
    IWrapper wrapper = wrapped.Proxify().Into<IWrapper>();
    Test(wrapper);
    
    IWrapper customWrapper = wrapped.Proxify()
          .Redirect(subject => subject.UnmatchedProperty)
          .Into<IWrapper>()
            .Property(proxy => proxy.NonMatch)
              .WithGetter(arg => int.Parse(arg))
              .WithSetter(arg => arg.ToString())
          .Proxy;
    TestNonMatch(customWrapper);
  }
  
  public static void Test(IWrapper wrapper) {
    wrapper.CallMethod();
    wrapper.Property = "Hi";
    string data = wrapper.Property;
  }

  public static void TestNonMatch(IWrapper wrapper) {
    wrapper.NonMatch = 1;
    int result = wrapper.NonMatch;
  }
}
````

###`Console output`

> Wrapped.CallMethod()  
> Wrapped.Property.get, with value: Hi  
> Wrapped.Property.set, with value: Hi  
> Wrapped.UnmatchedProperty.set, input value: 1  
> Wrapped.UnmatchedProperty.get, returned value: 1  