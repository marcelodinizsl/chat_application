using chat_application.Infra.CrossCutting.Bus;
using chat_application.Infra.Data.Context;
using chat_application.Infra.Data.Interface;
using chat_application.Infra.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace chat_application.Infra.CrossCutting.IoC
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMessageBroker, MessageBroker>();
            services.AddDbContext<ChatContext>();
        }
    }
}
