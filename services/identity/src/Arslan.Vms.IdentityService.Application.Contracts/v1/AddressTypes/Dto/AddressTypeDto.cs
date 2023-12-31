﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.IdentityService.v1.AddressTypes.Dto
{
    public class AddressTypeDto
    {
        public Guid? ParentId { get; set; }
        public Guid Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}