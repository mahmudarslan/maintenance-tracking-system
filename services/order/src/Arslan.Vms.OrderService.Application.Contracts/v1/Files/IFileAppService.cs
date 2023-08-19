﻿using Arslan.Vms.OrderService.v1.Files.Dtos;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.OrderService.v1.Files
{
    public interface IFileAppService
    {
        Task<Guid> CreateAsync(CreateFileAttachmentDto file);
        Task<FileAttachmentDto> GetAsync(Guid id);
    }
}