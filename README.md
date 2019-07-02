[![Build status](https://ci.appveyor.com/api/projects/status/djfbr3frv1nstkjo?svg=true)](https://ci.appveyor.com/project/fdipuma/nhapi)

# nHapi.Core
NHapi is a .NET port of the original Java project [HAPI](http://hl7api.sourceforge.net/). This project is a simplified fork of the original [NHapi](https://github.com/duaneedwards/nHapi) project developed by [Duane Edwards](https://github.com/duaneedwards), which removes some Framework dependencies in order to improve portability in .NET Standard and .NET Core projects.

NHapi allows Microsoft .NET developers to easily use an HL7 2.x object model. This object model allows for parsing and encoding HL7 2.x data to/from Pipe Delimited or XML formats. A very handy program for use in the health care industry.

This project is **NOT** affiliated with the HL7 organization. This software just conforms to the HL7 2.x specifications.

**Key Benefits**

- Easy object model  
- Microsoft .NET Standard 2.0 library that conforms to HL7 2.1, 2.2, 2.3, 2.3.1, 2.4, 2.5, 2.5.1, 2.6, 2.7, 2.7.1, 2.8 and 2.8.1 specifications  
- Can take a pipe delimited or XML formatted HL7 2.x message and build the C# object model for use in code  
- Can take the C# HL7 object model and produce pipe delimited or XML formatted HL7  
- FREE! (You can't beat that price) and open source  
- Fast  

## Requirements

NHapi.Core currently targets .NET Standard 2.0

## Getting Started

The easiest way to get started using nHapi is to use the [NuGet package 'nHapi.Core'](https://www.nuget.org/packages/nHapi.Core/):

Using the package manager console withing visual studio, simply run the following command:

```
PM > Install-Package nHapi.Core
```

## Differences from the original nHapi project

In order to simplify dependencies, this for does not implement some features originally available in nHapi:

- Model Generation and source generation have been completely stripped off. This also removes the need to take a dependency on `System.Data.Odbc`.
- The configuration is now handled from code, in order to remove the use of `System.Configuration.ConfigurationManager` (which uses unavailable APIs for .NET Core projects).

The last point means that, in order to add a custom package version, you now cannot rely on using `app.config`. You should configure packages at application startup, instead:

For instance:

```c#
using System;
public class Program
{
    public static void Main()
    {
		PackageManager.Instance.AddCustomVersion("NHapi.Model.V22_ZSegments", "2.2.CustomZ");
        // other application code
    }
}

```

## [Change Log](https://github.com/duaneedwards/nHapi/blob/master/CHANGELOG.md)