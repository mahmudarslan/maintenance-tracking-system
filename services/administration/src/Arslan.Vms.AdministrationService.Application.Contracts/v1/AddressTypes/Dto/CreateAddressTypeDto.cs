﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.AdministrationService.v1.AddressTypes.Dto
{
    public class CreateAddressTypeDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}