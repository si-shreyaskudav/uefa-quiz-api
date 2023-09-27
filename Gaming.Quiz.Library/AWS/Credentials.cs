using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace  Gaming.Quiz.Library.AWS
{
    public class Credentials
    {
        public static AWSCredentials _AWSCredentials
        {
            get
            {
                var chain = new CredentialProfileStoreChain();
                AWSCredentials awsCredentials;
                //read the below profile name from appsettings
                chain.TryGetAWSCredentials("sportz-interactive", out awsCredentials);
                //chain.TryGetAWSCredentials("saida-gaming", out awsCredentials);

                return awsCredentials;
            }
        }
    }
}
