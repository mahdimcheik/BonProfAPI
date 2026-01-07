namespace BonProf.Services;

using BonProf.Services.Interfaces;
using BonProf.Utilities;
using System.Net.Http.Headers;

public class SeaweedService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly string _filerUrl;
    private readonly ILogger<SeaweedService> _logger;

    public SeaweedService(HttpClient httpClient, IConfiguration configuration, ILogger<SeaweedService> logger)
    {
        _httpClient = httpClient;
        _filerUrl = EnvironmentVariables.FilerUrl;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderPath)
    {
        try
        {
            var requestUrl = $"{_filerUrl}/{folderPath.Trim('/')}/{fileName}";

            // Add authentication token if available
            if (!string.IsNullOrEmpty(TokenService.FilerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", TokenService.FilerToken);
            }

            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            
            // Set proper content type based on file extension
            var contentType = GetContentType(fileName);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = fileName
            };
            
            content.Add(streamContent, "file", fileName);

            _logger.LogInformation("Uploading file to: {Url}", requestUrl);
            var response = await _httpClient.PostAsync(requestUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Upload failed with status {StatusCode}: {Error}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException(
                    $"Upload failed with status {response.StatusCode}: {errorContent}");
            }

            response.EnsureSuccessStatusCode();
            _logger.LogInformation("File uploaded successfully: {FileName}", fileName);

            return requestUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", fileName);
            throw new Exception($"Erreur lors de l'upload du fichier: {ex.Message}", ex);
        }
    }

    public async Task<(Stream Content, string ContentType)> DownloadFileAsync(string filePath)
    {
        try
        {
            var requestUrl = $"{_filerUrl}/{filePath.TrimStart('/')}";
            
            // Add authentication token if available
            if (!string.IsNullOrEmpty(TokenService.FilerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", TokenService.FilerToken);
            }

            _logger.LogInformation("Downloading file from: {Url}", requestUrl);
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

            return (stream, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FilePath}", filePath);
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var requestUrl = $"{_filerUrl}/{filePath.TrimStart('/')}";
            
            // Add authentication token if available
            if (!string.IsNullOrEmpty(TokenService.FilerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", TokenService.FilerToken);
            }

            _logger.LogInformation("Deleting file: {Url}", requestUrl);
            var response = await _httpClient.DeleteAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
            }
            else
            {
                _logger.LogWarning("Failed to delete file {FilePath}: {StatusCode}", 
                    filePath, response.StatusCode);
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            return false;
        }
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".zip" => "application/zip",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            _ => "application/octet-stream"
        };
    }
}

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;
    private readonly string _filerUrl;
    private readonly ILogger<FileService> _logger;

    public FileService(HttpClient httpClient, IConfiguration configuration, ILogger<FileService> logger)
    {
        _httpClient = httpClient;
        _filerUrl = EnvironmentVariables.FilerUrl;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderPath)
    {
        try
        {
            var requestUrl = $"{_filerUrl}/files/upload";

            // Add authentication token if available
            if (!string.IsNullOrEmpty(TokenService.FilerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", TokenService.FilerToken);
            }

            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            
            // Set proper content type based on file extension
            var contentType = GetContentType(fileName);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            
            // Important: Set proper Content-Disposition header for IFormFile
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",  // This must match the parameter name in the API
                FileName = fileName
            };

            content.Add(streamContent, "file", fileName);

            // Add folder path as form data if needed
            if (!string.IsNullOrEmpty(folderPath))
            {
                content.Add(new StringContent(folderPath), "folder");
            }

            _logger.LogInformation("Uploading file to: {Url}", requestUrl);
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Upload failed with status {StatusCode}: {Error}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException(
                    $"Upload failed with status {response.StatusCode}: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("File uploaded successfully: {FileName}. Response: {Response}", 
                fileName, responseContent);

            // Parse response to get actual file URL if the API returns it
            return responseContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", fileName);
            throw new Exception($"Erreur lors de l'upload du fichier: {ex.Message}", ex);
        }
    }

    public async Task<(Stream Content, string ContentType)> DownloadFileAsync(string filePath)
    {
        try
        {
            var requestUrl = $"{_filerUrl}/{filePath.TrimStart('/')}";
            
            // Add authentication token if available
            if (!string.IsNullOrEmpty(TokenService.FilerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", TokenService.FilerToken);
            }

            _logger.LogInformation("Downloading file from: {Url}", requestUrl);
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

            return (stream, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FilePath}", filePath);
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var requestUrl = $"{_filerUrl}/{filePath.TrimStart('/')}";
            
            // Add authentication token if available
            if (!string.IsNullOrEmpty(TokenService.FilerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", TokenService.FilerToken);
            }

            _logger.LogInformation("Deleting file: {Url}", requestUrl);
            var response = await _httpClient.DeleteAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
            }
            else
            {
                _logger.LogWarning("Failed to delete file {FilePath}: {StatusCode}", 
                    filePath, response.StatusCode);
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            return false;
        }
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".zip" => "application/zip",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            _ => "application/octet-stream"
        };
    }
}

