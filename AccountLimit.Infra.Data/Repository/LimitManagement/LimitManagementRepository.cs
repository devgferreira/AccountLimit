using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using AccountLimit.Infra.Data.Entities.LimitManagement;
using AccountLimit.Infra.Data.Mapping.LimitManagement;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task DeleteLimitManagement(string cpf)
        {
            await _context.DeleteAsync<LimitManagementEntity>(cpf);
        }
        public async Task<List<LimitManagementInfo>> SelectLimitManagement(LimitManagementRequest request)
        {
            var entities = await _context
                .QueryAsync<LimitManagementEntity>(request.Cpf)
                .GetRemainingAsync();

            var result = new List<LimitManagementInfo>();

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
