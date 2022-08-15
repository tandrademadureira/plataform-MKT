using Shared.Domain.SeedWork;
using Shared.Util.Result;
using System;

namespace Plataform.MKT.Domain.AggregateModels.Products
{
    public class Product : Entity, IAggregateRoot
    {
        private Product()
        {

        }

        public static Result<Product> CreateProduct(string description, string mark, Guid id)
        {
            if (string.IsNullOrEmpty(description))
            {
                return Result.Fail<Product>("Description must not be null");
            }

            if (string.IsNullOrEmpty(mark))
            {
                return Result.Fail<Product>("Mark must not be null");
            }

            var model = new Product
            {
                Id = id,
                Description = description,
                Mark = mark,
                CreatedAt = DateTimeOffset.Now,
                Approved = false,
                DataRequisition = null,
                Amount = 0
            };

            return Result.Ok(model);
        }

        public string Description { get; private set; }
        public string Mark { get; private set; }
        public bool Approved { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset? DataRequisition { get; set; }
    }
}
