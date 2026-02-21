using AccountLimit.Application.Interface.LimitManagement;
using AccountLimit.Application.Interface.TransactionAuthorization;
using AccountLimit.Application.Service.LimitManagement;
using AccountLimit.Application.Service.TransactionAuthorization;
using AccountLimit.Domain.Interface;
using AccountLimit.Infra.Data.Repository.LimitManagementRepository;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
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
            services.AddSingleton<IAmazonDynamoDB>(_ =>
            {
                var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
                var region = Environment.GetEnvironmentVariable("AWS_REGION");

                if (string.IsNullOrWhiteSpace(accessKey) ||
                    string.IsNullOrWhiteSpace(secretKey) ||
                    string.IsNullOrWhiteSpace(region))
                {
                    throw new InvalidOperationException("AWS environment variables not loaded. Check your .env file.");
                }

                var credentials = new BasicAWSCredentials(accessKey, secretKey);

                var config = new AmazonDynamoDBConfig
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(region)
                };

                return new AmazonDynamoDBClient(credentials, config);
            });

            services.AddSingleton<IDynamoDBContext>(sp =>
            {
                var client = sp.GetRequiredService<IAmazonDynamoDB>();
                return new DynamoDBContext(client);
            });
            services.AddScoped<ILimitManagementRepository, LimitManagementRepository>();
            services.AddScoped<ILimitManagementService, LimitManagementService>();
            services.AddScoped<ITransactionAuthorizationService, TransactionAuthorizationService>();


            return services;
        }
    }
}
