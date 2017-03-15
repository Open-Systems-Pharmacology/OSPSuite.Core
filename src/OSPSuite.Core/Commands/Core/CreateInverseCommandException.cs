using System;

namespace OSPSuite.Core.Commands.Core
{
    public class CreateInverseCommandException : Exception
    {
        private static string _createInverseCommandMessage = "Cannot create inverse command";

        public CreateInverseCommandException():base(_createInverseCommandMessage)
        {
        }
    }
}