﻿using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class OrderStatusRepository : GenericRepository<OrderStatus>, IOrderStatusRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderStatusRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddDefaultOrderStatusAsync()
    {
        ICollection<OrderStatus> ordersStatus = new HashSet<OrderStatus>()
        {
            new OrderStatus(){StatusName = "Processing" },
            new OrderStatus(){StatusName = "Shipped" },
            new OrderStatus(){StatusName = "Delivered" },
            new OrderStatus(){StatusName = "Canceled" }
        };

        AddRange(ordersStatus);

        await _unitOfWork.SaveChangesAsync();
    }
}
