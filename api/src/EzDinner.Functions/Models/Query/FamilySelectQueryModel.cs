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
    public class FamilySelectQueryModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public class FamilySelectMapping : Profile
    {
        public FamilySelectMapping()
        {
            CreateMap<Family, FamilySelectQueryModel>();
        }
    }
}
