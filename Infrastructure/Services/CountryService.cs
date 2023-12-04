using Domain.Guards;
using Application.Common.Interfaces;
using Domain.Common.Request;
using System.Net.Http.Json;
using Domain.Guards.Extensions;

namespace Infrastructure.Services;

public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;

    public CountryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ICollection<CountryRequest>> GetCountriesAsync()
    {
        ICollection<CountryRequest>? request = await _httpClient.GetFromJsonAsync<ICollection<CountryRequest>>("");

        Guard.Against.NullOrEmpty(request, nameof(request), $"The request maded return no items");

        return request;
    }
}
