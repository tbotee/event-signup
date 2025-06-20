using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.SecretsManager;
using Constructs;

namespace LegoEventSignup.Resources
{
    public class DatabaseResources
    {
        public DatabaseInstance DatabaseInstance { get; }

        public DatabaseResources(Construct scope, Vpc vpc)
        {
            var credentialsSecret = new DatabaseSecret(scope, "LegoEventSignupPostgresSecret", new DatabaseSecretProps
            {
                Username = "postgres"
            });

            DatabaseInstance = new DatabaseInstance(scope, "LegoEventSignupPostgresDatabase", new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.Postgres(new PostgresInstanceEngineProps { Version = PostgresEngineVersion.VER_15_13 }),
                InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.MICRO),
                Vpc = vpc,
                VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PRIVATE_WITH_EGRESS },
                Credentials = Credentials.FromSecret(credentialsSecret),
                MultiAz = false,
                AllocatedStorage = 20,
                MaxAllocatedStorage = 100,
                BackupRetention = Duration.Seconds(0),
                DeletionProtection = false,
                RemovalPolicy = RemovalPolicy.DESTROY,
                PubliclyAccessible = false,
                DatabaseName = "legoeventdb"
            });
        }
    }
}