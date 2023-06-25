﻿using PPO2.Core.DTOs.Storage;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core.Interfaces
{
    public interface IStorageService
    {
        Task<ServiceResponse> ValidatePrimaryKeysAsync(int productId, int projectId);
        Task<ServiceResponse> CreateAsync(StorageCreateDto storageDto);
    }
}
