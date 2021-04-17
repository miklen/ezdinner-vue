using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Core.Aggregates
{
    public abstract class AggregateRoot<TIdType>
    {
        /// <summary>
        /// The aggregate root's unique Id.
        /// </summary>
        public TIdType Id { get; set; }
        
        /// <summary>
        /// The partial key is a necessity for CosmosDb to distribute load.
        /// The most even distribution is by global unique Id and chosen as the default here.
        /// It can be overriden in an aggregate if another distribution makes better sense.
        /// </summary>
        public virtual TIdType PartitionKey => Id;

        public AggregateRoot(TIdType id)
        {
            Id = id;
        }
    }
}
