using Application.Common.Interfaces;
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

    public async Task AddDefaultCountries()
    {
        var result = await _countryService.GetCountries();

        ICollection<Country> countries = result.Select(country => new Country() {CountryName = country.Name.Common }).ToHashSet();

        _context.Country.AddRange(countries);

        await _unitOfWork.SaveChangesAsync();
    }
}
