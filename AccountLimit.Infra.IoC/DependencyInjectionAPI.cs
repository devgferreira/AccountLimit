using AccountLimit.Application.Interface.LimitManagement;
using AccountLimit.Application.Service.LimitManagement;
using AccountLimit.Domain.Interface;
using AccountLimit.Infra.Data.Repository.LimitManagementRepository;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            services.AddScoped<ILimitManagementRepository, LimitManagementRepository>();
            services.AddScoped<ILimitManagementService, LimitManagementService>();


            return services;
        }
    }
}
