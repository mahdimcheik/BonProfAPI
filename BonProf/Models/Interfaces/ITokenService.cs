namespace BonProf.Services.Interfaces;

public interface ITokenService
{
    Task GetAsync(string serviceName);
}