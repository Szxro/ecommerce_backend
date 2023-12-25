using Application.Common.Interfaces;
using Application.Common.Mapping;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICountryService _countryService;

    public CountryRepository(AppDbContext context,
                             IUnitOfWork unitOfWork,
                            ICountryService countryService) : base(context)
    {
        _unitOfWork = unitOfWork;
        _countryService = countryService;
    }

    public async Task AddDefaultCountriesAsnyc()
    {
        var result = await _countryService.GetCountriesAsync();

        ICollection<Country> countries = result.ToCountry();

        AddRange(countries);

        await _unitOfWork.SaveChangesAsync();
    }
}
