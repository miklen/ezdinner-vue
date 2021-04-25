using AutoMapper;
using EzDinner.Core.Aggregates.FamilyAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Functions.Models.Query
{
    /// <summary>
    /// Model to display the family. Both as cards and in dropdown.
    /// </summary>
    public class FamilyQueryModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid OwnerId { get; set; }
        public string? OwnerName { get; set; }
    }

    public class FamilyMapping : Profile
    {
        public FamilyMapping()
        {
            CreateMap<Family, FamilyQueryModel>();
        }
    }
}
