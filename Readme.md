## SFTP.Wrapper

> This package is a wrapper for SSH.Net. The SFTP features are wrapped in an easy to use request/response design.

## Usage ##

* Install the nuget package
> `Install-Package SFTP.Wrapper`

* Register your services

```CSharp
private static void RegisterServices(IServiceCollection services)
{
    //
    // Register services here
    //
    services.AddLogging();
    services.UseSftp(new SftpConfig
    {
        Host = "HOST",
        UserName = "USER NAME",
        Password = "PASSWORD",
        Port = 22
    });
}
```

* The package exposes `ISftpManager` interface, inject it to your class.

```CSharp
//
// Example
//
public class SomeService
{
    private readonly ISftpManager _sftpManager;
    private readonly ILogger<SomeService> _logger;

    public SomeService(ISftpManager sftpManager, ILogger<SomeService> logger)
    {
        _logger = logger;
        _sftpManager = sftpManager;        
    }

    public async Task<GetAllFilesResponse> SomeMethod()
    {
        var listOperation = await _sftpManager.GetAllFilesAsync(new GetAllFilesRequest(@"/home")).ConfigureAwait(false);
        if (listOperation.Status)
        {
            return listOperation.Data;
        }
        else
        {            
            _logger.LogError(listOperation.Message);            
        }
    }
}

```
