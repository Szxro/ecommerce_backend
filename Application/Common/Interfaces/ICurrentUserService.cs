﻿namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? GetCurrentUsername();

    List<string>? GetCurrentUserRoles();
}
