using System;
using System.Threading.Tasks;

using R5T.F0000;
using R5T.L0029;
using R5T.T0132;


namespace R5T.S0058.Deploy
{
    [FunctionalityMarker]
    public partial interface IDeployScripts : IFunctionalityMarker
    {
        public async Task DeployToLocalCloudBinariesDirectory()
        {
            /// Inputs.
            var targetProjectName = "R5T.S0058";
            var localCloudBinariesDirectoryPath = Z0024.DirectoryPaths.Instance.LocalCloudBinariesDirectoryPath;


            /// Run.
            var targetProjectFilePath = F0026.ProjectPathConventions.Instance.GetExecutableSiblingProjectFilePath(targetProjectName);

            var currentBinariesDirectoryPath = await F0035.LoggingOperator.Instance.InConsoleLoggerContext(
                nameof(DeployToLocalCloudBinariesDirectory),
                async logger =>
                {
                    var currentBinariesDirectoryPath = await L0028.PublicationOperator.Instance.Publish(
                        targetProjectFilePath,
                        localCloudBinariesDirectoryPath,
                        logger);

                    return currentBinariesDirectoryPath;
                });

            F0034.WindowsExplorerOperator.Instance.OpenDirectoryInExplorer(currentBinariesDirectoryPath);
        }

        /// <summary>
        /// Requires <see cref="DeployToLocalCloudBinariesDirectory"></see> to be run first.
        /// </summary>
        public async Task DeployToRemoteBinariesDirectory()
        {
            /// Inputs.
            var targetProjectName = "R5T.S0058";
            var remoteServerFriendlyName = "TechnicalBlog";
            var remoteDeployDirectoryPath = $@"/var/www/{targetProjectName}";

            var archiveFileName = @"Archive.zip";
            var localTemporaryDirectoryPath = @"C:\Temp";
            var remoteTemporaryDirectoryPath = @"/home/ec2-user";
            var localCloudBinariesDirectoryPath = Z0024.DirectoryPaths.Instance.LocalCloudBinariesDirectoryPath;


            /// Run.
            var awsRemoteServerAuthentication = F0096.AwsAuthenticationOperator.Instance.GetRemoteServerAuthentication(
                Z0017.FilePaths.Instance.AwsRemoteServerConfigurationJsonFilePath,
                remoteServerFriendlyName);

            var targetProjectFilePath = F0026.ProjectPathConventions.Instance.GetExecutableSiblingProjectFilePath(targetProjectName);

            var currentBinariesDirectoryPath = L0028.PublicationPathsOperator.Instance.GetCurrentBinariesOutputDirectoryPath(
                localCloudBinariesDirectoryPath,
                targetProjectFilePath);

            var remoteDeployContext = new RemoteDeployContext
            {
                ArchiveFileName = archiveFileName,
                DestinationRemoteBinariesDirectoryPath = remoteDeployDirectoryPath,
                LocalTemporaryDirectoryPath = localTemporaryDirectoryPath,
                RemoteTemporaryDirectoryPath = remoteTemporaryDirectoryPath,
                SourceLocalBinariesDirectoryPath = currentBinariesDirectoryPath,
            };

            await F0035.LoggingOperator.Instance.InConsoleLoggerContext(
                nameof(DeployToLocalCloudBinariesDirectory),
                async logger =>
                {
                    await DeployOperator.Instance.DeployToRemote(
                        awsRemoteServerAuthentication,
                        remoteDeployContext,
                        EnumerableOperator.Instance.From(
                            RemoteDeployActions.Instance.None),
                        EnumerableOperator.Instance.From(
                            RemoteDeployActions.Instance.ChangePermissionsOnRemoteDirectory(logger)),
                        logger);
                });
        }
    }
}
