## Installation

Use the Nuget package manager [SmartShareClient](https://www.nuget.org/packages/SmartShareClient) to install.

```bash
dotnet add package SmartShareClient
```

## Usage

```csharp
services.AddSmartShare(options => 
{
	options.Endpoint = "";
	options.ClientId = "";
	options.ClientKey = "";
	options.User = "";
	options.Password = "";
});
```

```csharp
private readonly ISmartShare smartShareClient;

public Test(ISmartShare smartShareClient)
{
	this.smartShareClient = smartShareClient;
}

public async Task Foo()
{
	this.smartShareClient.UploadDocumentAsync(...);
}
```

## License
[MIT](https://choosealicense.com/licenses/mit/)