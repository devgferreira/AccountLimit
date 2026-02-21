using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using AccountLimit.Infra.Data.Entities.LimitManagement;
using AccountLimit.Infra.Data.Mapping.LimitManagement;
using Amazon.DynamoDBv2.DataModel;

namespace AccountLimit.Infra.Data.Repository.LimitManagementRepository
{
    public class LimitManagementRepository : ILimitManagementRepository
    {

        private readonly IDynamoDBContext _context;
        public LimitManagementRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task CreateLimitManagement(LimitManagementInfo request)
        {
            await _context.SaveAsync(request.ToEntity());
        }

        public async Task DeleteLimitManagement(string cpf, string agency)
        {
            await _context.DeleteAsync<LimitManagementEntity>(cpf, agency);
        }
        public async Task<List<LimitManagementInfo>> SelectLimitManagement(LimitManagementRequest request)
        {
            List<LimitManagementEntity> entities;

            if (string.IsNullOrWhiteSpace(request.Agency))
            {
                entities = await _context
                    .QueryAsync<LimitManagementEntity>(request.Cpf)
                    .GetRemainingAsync();
            }
            else
            {
                var entity = await _context.LoadAsync<LimitManagementEntity>(request.Cpf, request.Agency);

                entities = entity is null
                    ? new List<LimitManagementEntity>()
                    : new List<LimitManagementEntity> { entity };
            }

            var result = new List<LimitManagementInfo>(entities.Count);

            foreach (var e in entities)
            {
                var domainResult = e.ToDomain();
                if (domainResult.IsFailure)
                    throw new Exception(domainResult.Error);

                result.Add(domainResult.Value);
            }

            return result;
        }
        public async Task UpdateLimitManagement(LimitManagementInfo request)
        {
            await _context.SaveAsync(request.ToEntity());
        }
    }
}
