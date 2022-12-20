using System;


namespace R5T.S0058.Deploy
{
    public class DeployScripts : IDeployScripts
    {
        #region Infrastructure

        public static IDeployScripts Instance { get; } = new DeployScripts();


        private DeployScripts()
        {
        }

        #endregion
    }
}
