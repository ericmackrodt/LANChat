using Autofac;
using LANChat.Client;
using LANChat.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANChat.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IContainer _container;

        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<ChatClient>().As<IChatClient>();
            containerBuilder.RegisterType<ChatServer>().As<IChatServer>();

            containerBuilder.RegisterType<MainViewModel>();
            containerBuilder.RegisterType<ServerViewModel>().SingleInstance();

            _container = containerBuilder.Build();
        }

        public MainViewModel MainViewModel
        {
            get { return _container.Resolve<MainViewModel>(); }
        }

        public ServerViewModel ServerViewModel
        {
            get { return _container.Resolve<ServerViewModel>(); }
        }
    }
}
