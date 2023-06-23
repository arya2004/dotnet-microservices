﻿using Mango.Services.EmailAPI.Messaging;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mango.Services.EmailAPI.Extension
{
    public static class ApplicaioBuilderExtensions
    {   
        private static IAzureServiceBusConsumer azureServiceBusConsumer { get;set; }
        public static IApplicationBuilder USeAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            azureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostAppLife = app.ApplicationServices.GetService<IHostApplicationLifetime> ();

            hostAppLife.ApplicationStarted.Register(OnStart);
            hostAppLife.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStop()
        {
            azureServiceBusConsumer.Stop ();
        }

        private static void OnStart()
        {
            azureServiceBusConsumer.Start ();
        }
    }
}
