using System;
using System.Linq;

namespace PI.WebGarten.MethodBasedCommands
{
    public class DefaultMethodBasedCommandFactory
    {
        private static readonly IParameterBinder Binder = new CompositeParameterBinder(
                new UriTemplateParameterBinder(),
                new RequestParameterBinder(),
                new FormUrlEncodingParameterBinder(),
                new UserParameterBinder() // Para permitir receber o IPrincipal nos CommandMethods
        );

        public static ICommand[] GetCommandsFor(params Type[] types)
        {
            return new MethodBasedCommandFactory(Binder, types).Create().ToArray();
        }
    }
}
