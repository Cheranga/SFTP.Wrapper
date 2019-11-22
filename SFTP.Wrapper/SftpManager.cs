using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using SFTP.Wrapper.Configs;
using SFTP.Wrapper.Models;
using SFTP.Wrapper.Requests;
using SFTP.Wrapper.Responses;

namespace SFTP.Wrapper
{
    public class SftpManager : ISftpManager
    {
        private readonly SftpConfig _config;
        private readonly ILogger<SftpManager> _logger;
        private readonly bool _isLoggingEnabled = false;

        public SftpManager(SftpConfig config, ILogger<SftpManager> logger = null)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config), "Config cannot be null");
            _logger = logger;
            _isLoggingEnabled = _logger != null;
        }

        public virtual async Task<ResultStatus<GetAllFilesResponse>> GetAllFilesAsync(GetAllFilesRequest request)
        {
            var response = await HandleAsync(async (client, req) =>
            {
                var fileList = await Task.Factory.FromAsync(
                        client.BeginListDirectory(request.Directory, null, null), client.EndListDirectory)
                    .ConfigureAwait(false);

                var transformedFileList = (fileList?.ToList() ?? new List<SftpFile>()).Select(x => new SftpFileInformation(x.Name, x.FullName, x.Length)).ToList();

                return new GetAllFilesResponse(transformedFileList);
            }, request, nameof(GetAllFilesAsync)).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<ResultStatus<DownloadFileResponse>> DownloadFileAsync(DownloadFileRequest request)
        {
            var response = await HandleAsync(async (client, req) =>
            {
                var stream = new MemoryStream();
                await Task.Factory.FromAsync(client.BeginDownloadFile(request.File, stream), client.EndDownloadFile).ConfigureAwait(false);

                return new DownloadFileResponse
                {
                    Status = true,
                    FileName = Path.GetFileName(request.File),
                    Stream = stream
                };
            }, request, nameof(DownloadFileAsync)).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<ResultStatus<BulkDownloadFileResponse>> BulkDownloadFilesAsync(BulkDownloadFileRequest request)
        {
            var response = await HandleAsync(async (client, req) =>
            {
                var downloadedFileResponses = new List<DownloadFileResponse>();

                var tasks = request.Requests.Select(x =>
                {
                    var stream = new MemoryStream();
                    return Task.Factory.FromAsync(
                        (callback, obj) =>
                            client.BeginDownloadFile(x.File, stream, callback, obj),
                        result =>
                        {
                            client.EndDownloadFile(result);
                            if (result.IsCompleted)
                            {
                                downloadedFileResponses.Add(new DownloadFileResponse
                                {
                                    Stream = stream,
                                    FileName = x.File,
                                    Status = true
                                });
                            }
                        }, null);
                });

                await Task.WhenAll(tasks).ConfigureAwait(false);

                return new BulkDownloadFileResponse(downloadedFileResponses);
            }, request, nameof(BulkDownloadFilesAsync)).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<ResultStatus<DeleteFileResponse>> DeleteFileAsync(DeleteFileRequest request)
        {
            var response = await HandleAsync(async (client, req) =>
            {
                await Task.Factory.StartNew(() => client.Delete(request.File)).ConfigureAwait(false);

                return new DeleteFileResponse(request.File);
            }, request, nameof(DeleteFileAsync)).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<ResultStatus<DeleteDirectoryResponse>> DeleteDirectoryAsync(DeleteDirectoryRequest request)
        {
            var response = await HandleAsync(async (client, req) =>
            {
                await Task.Factory.StartNew(() => client.DeleteDirectory(request.Directory)).ConfigureAwait(false);

                return new DeleteDirectoryResponse(request.Directory);
            }, request, nameof(DeleteDirectoryAsync)).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<ResultStatus<UploadFileResponse>> UploadFileAsync(UploadFileRequest request)
        {
            var response = await HandleAsync(async (client, req) =>
            {
                await Task.Factory.FromAsync(client.BeginUploadFile(request.StreamToUpload, request.WhereToUpload), client.EndUploadFile).ConfigureAwait(false);

                return new UploadFileResponse(request.WhereToUpload);
            }, request, nameof(UploadFileAsync)).ConfigureAwait(false);

            return response;
        }

        public virtual async Task<ResultStatus<TResponse>> HandleAsync<TRequest, TResponse>(Func<SftpClient, TRequest, Task<TResponse>> operation, TRequest request, string nameOfOperation)
        where TRequest : class, IValidatable where TResponse : class
        {
            var isValid = request?.IsValid() ?? false;

            if (!isValid)
            {
                Log(LogLevel.Error, "Invalid Request");
                return ResultStatus<TResponse>.Error("Invalid request");
            }

            if (operation == null)
            {
                Log(LogLevel.Error, "Please specify the operation to handle");
                return ResultStatus<TResponse>.Error("Please specify the response to handle");
            }

            if (string.IsNullOrWhiteSpace(nameOfOperation))
            {
                Log(LogLevel.Error, "Please specify a name for the operation");
                return ResultStatus<TResponse>.Error("Please specify a name for the operation");
            }

            try
            {
                using (var client = new SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password))
                {
                    try
                    {
                        client.Connect();

                        var response = await operation(client, request).ConfigureAwait(false);
                        Log(LogLevel.Information, $"{nameOfOperation} executed successfully");
                        return ResultStatus<TResponse>.Success(response);
                    }
                    catch (Exception exception)
                    {
                        var message = exception.Message ?? $"Error occured in {nameOfOperation}";
                        Log(LogLevel.Error, message);
                        return ResultStatus<TResponse>.Error(message, exception);
                    }
                    finally
                    {
                        client.Disconnect();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = exception.Message ?? "Cannot connect to the SFTP host";
                Log(LogLevel.Error, message);
                return ResultStatus<TResponse>.Error(message, exception);
            }
        }

        private void Log(LogLevel level, string message)
        {
            if (_isLoggingEnabled && !string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            switch (level)
            {
                case LogLevel.Critical:
                    _logger.LogCritical(message);
                    break;

                case LogLevel.Debug:
                    _logger.LogDebug(message);
                    break;
                case LogLevel.Error:
                    _logger.LogDebug(message);
                    break;
                case LogLevel.Information:
                    _logger.LogInformation(message);
                    break;
                case LogLevel.Trace:
                    _logger.LogTrace(message);
                    break;
                case LogLevel.Warning:
                    _logger.LogWarning(message);
                    break;

                default:
                    _logger.LogInformation(message);
                    break;
            }
        }
    }
}