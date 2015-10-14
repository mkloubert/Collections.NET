# Collections.NET

A set of collection classes written in C#.

| Name  | Targets on  |
| ----- | ----------- |
| [master](https://github.com/mkloubert/Collections.NET)  | C# 4.0  |
| [CSharp5](https://github.com/mkloubert/Collections.NET/tree/CSharp5)  | C# 5.0, .NET 4.5  |
| Portable8 (current)  | C# 4.0, .NET 4.5, Silverlight 5, Windows 8, Windows Phone 8.1 + 8 (Silverlight)  |

## Installation

Visit [NuGet site](https://www.nuget.org/packages/MarcelJoachimKloubert.Collections.dll) or enter the following command:

```powershell
Install-Package MarcelJoachimKloubert.Collections.dll 
```

## Thread safe classes

The following implementions exist (all collections are also wrappers for [INotifyPropertyChanged](https://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged%28v=vs.110%29.aspx) and [INotifyCollectionChanged](https://msdn.microsoft.com/en-us/library/system.collections.specialized.inotifycollectionchanged%28v=vs.110%29.aspx) interface):

### SynchronizedCollection&lt;T&gt;

This is an implementation or wrapper of/for a thread safe [ICollection&lt;T&gt;](https://msdn.microsoft.com/en-us/library/92t2ye13%28v=vs.110%29.aspx) object.

```csharp
// new / empty collection
var newColl = new SynchronizedCollection<object>();

// wrapper
var threadSafeColl = new SynchronizedCollection<string>(coll: new List<string>());
```

### SynchronizedDictionary&lt;TKey, TValue&gt;

This is an implementation or wrapper of/for a thread safe [IDictionary&lt;TKey, TValue&gt;](https://msdn.microsoft.com/en-us/library/s4ys34ea%28v=vs.110%29.aspx) object.

```csharp
// new / empty dictionary
var newDict = new SynchronizedDictionary<string, object>();

// wrapper
var threadSafeSetDict = new SynchronizedDictionary<int, object>(set: new Dictionary<int, object>());
```

### SynchronizedEnumerator&lt;T&gt;

This is a wrapper for the thread safe use of an [IEnumerator&lt;T&gt;](https://msdn.microsoft.com/en-us/library/78dfe2yb%28v=vs.110%29.aspx) object.

```csharp
var list = new List<double>();

// wrap enumerator of 'list' into a thread safe wrapper
using (var threadSafeEnumerator = new SynchronizedEnumerator<double>(list.GetEnumerator())) {
    // TODO
}
```

### SynchronizedList&lt;T&gt;

This is an implementation or wrapper of/for a thread safe [IList&lt;T&gt;](https://msdn.microsoft.com/en-us/library/5y536ey6%28v=vs.110%29.aspx) object.

```csharp
// new / empty list
var newList = new SynchronizedList<int>();

// wrapper
var threadSafeList = new SynchronizedList<float>(list: new List<float>());
```

### SynchronizedSet&lt;T&gt;

This is an implementation or wrapper of/for a thread safe [ISet&lt;T&gt;](https://msdn.microsoft.com/en-us/library/dd412081%28v=vs.110%29.aspx) object.

```csharp
// new / empty set
var newSet = new SynchronizedSet<string>();

// wrapper
var threadSafeSet = new SynchronizedSet<object>(set: new HashSet<object>());
```
