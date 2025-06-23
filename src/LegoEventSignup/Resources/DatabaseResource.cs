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

        public DatabaseSecret DatabaseSecret { get; }

        public const string DB_NAME = "legoeventdb";

        public DatabaseResources(Construct scope, Vpc vpc)
        {
            DatabaseSecret = new DatabaseSecret(scope, "LegoEventSignupPostgresSecret", new DatabaseSecretProps
            {
                Username = "postgres"
            });

            // Create a security group for the database
            var dbSecurityGroup = new SecurityGroup(scope, "DatabaseSecurityGroup", new SecurityGroupProps
            {
                Vpc = vpc,
                Description = "Security group for Lego Event Signup PostgreSQL database",
                AllowAllOutbound = true
            });

            // Allow PostgreSQL connections from anywhere
            dbSecurityGroup.AddIngressRule(
                Peer.AnyIpv4(),
                Port.Tcp(5432),
                "Allow PostgreSQL connections from anywhere"
            );

            DatabaseInstance = new DatabaseInstance(scope, "LegoEventSignupPostgresDatabase", new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.Postgres(new PostgresInstanceEngineProps { Version = PostgresEngineVersion.VER_15_13 }),
                InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.MICRO),
                Vpc = vpc,
                VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PUBLIC },
                Credentials = Credentials.FromSecret(DatabaseSecret),
                MultiAz = false,
                AllocatedStorage = 20,
                MaxAllocatedStorage = 100,
                BackupRetention = Duration.Seconds(0),
                DeletionProtection = false,
                RemovalPolicy = RemovalPolicy.DESTROY,
                PubliclyAccessible = true,
                SecurityGroups = new[] { dbSecurityGroup },
                DatabaseName = DB_NAME
            });
        }
    }
}