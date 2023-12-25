using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryRepository(AppDbContext context,IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddDefaultCategoriesAsync()
    {
        ICollection<Category> categories = new HashSet<Category>()
        {
           new Category(){CategoryName =  "Mobile phones",Description = "Mobile phones"},
           new Category(){CategoryName = "Game Consoles",Description = "Game Consoles" },
           new Category(){CategoryName = "Household furniture",Description = "Household furniture" },
           new Category(){CategoryName = "Home appliances",Description = "Home appliances" },
           new Category(){CategoryName = "Clothing",Description = "Clothing" }
        };

        AddRange(categories);

        await _unitOfWork.SaveChangesAsync();
    }
}
