using Application.Common.Guards;
using Application.Common.Interfaces;
using Domain.Common.Request;
using System.Net.Http.Json;

namespace Infrastructure.Services;

public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;

    public CountryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ICollection<CountryRequest>> GetCountries()
    {
        ICollection<CountryRequest>? request = await _httpClient.GetFromJsonAsync<ICollection<CountryRequest>>("");

        Ensure.NotNull(request);

        return request!;
    }
}
