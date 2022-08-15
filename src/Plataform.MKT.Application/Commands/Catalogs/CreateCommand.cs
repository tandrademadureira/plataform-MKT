using Shared.Infra.Cqrs;
using Shared.Util.Result;
using Plataform.MKT.Infra.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;
using Plataform.MKT.Infra.Data.Repositories;
using Plataform.MKT.Domain.AggregateModels.Products;
using MediatR;
using Plataform.MKT.Application.Events;
using System;

namespace Plataform.MKT.Application.Commands.Catalogs
{
    public class CreateCommand
    {
        public class CreateContract : BaseCommand<Result>
        {
            public Guid Id { get; set; }
            public string Description { get; set; }
            public string Mark { get; set; }
        }

        public class Handler : BaseHandler<CreateContract, Result>
        {
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMediator _mediator;

            public Handler(IProductRepository productRepository, 
                           IUnitOfWork unitOfWork, 
                           IMediator mediator)
            {
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
                _mediator = mediator;
            }

            public async override Task<Result> Handle(CreateContract request, CancellationToken cancellationToken)
            {                
                var resultModel = Product.CreateProduct(request.Description, request.Mark, request.Id);

                if (resultModel.IsFailure)
                    return Result.Fail(resultModel.Error);

                await _productRepository.Add(resultModel.Data);
                await _unitOfWork.SaveChangesAsync();

                await _mediator.Publish(new SendQueueEvent(request.Description, request.Mark, request.Id));

                return Result.Ok();
            }
        }
    }
}
