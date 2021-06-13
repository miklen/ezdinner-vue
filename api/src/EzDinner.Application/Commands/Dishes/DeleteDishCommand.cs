using EzDinner.Core.Aggregates.DishAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Application.Commands.Dishes
{
    public class DeleteDishCommand
    {
        private readonly IDishRepository _dishRepository;

        public DeleteDishCommand(IDishRepository dishRespository)
        {
            _dishRepository = dishRespository;
        }

        public async Task Handle(Guid familyId, Guid dishId)
        {
            if (familyId == Guid.Empty) throw new ArgumentException("MISSING_FAMILYID");
            if (dishId == Guid.Empty) throw new ArgumentException("MISSING_FAMILYID");

            var dish = await _dishRepository.GetDishAsync(dishId);
            if (dish is null) return;
            dish.Delete();
            await _dishRepository.SaveAsync(dish);
        }
    }
}
