using AutoMapper;
using EzDinner.Core.Aggregates.DinnerAggregate;
using EzDinner.Core.Aggregates.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using NodaTime;

namespace EzDinner.Functions.Models.Migration
{
    public class LegacyDateTimeDinner
    {
        public Guid Id { get; set; }
        public Guid PartitionKey { get; set; }
        public Guid FamilyId { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<MenuItem> Menu { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public bool IsPlanned { get; set; }

        internal object Migrate()
        {
            var dinner = new Dinner(FamilyId, new LocalDate(Date.Year, Date.Month, Date.Day));
            dinner.Id = Id;
            foreach (var menuItem in Menu) dinner.AddMenuItem(menuItem.DishId, menuItem.ReciepeId);
            return dinner;
        }
    }
}
