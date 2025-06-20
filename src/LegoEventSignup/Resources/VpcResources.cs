using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Constructs;

namespace LegoEventSignup.Resources
{
    public class VpcResources
    {
        public Vpc Vpc { get; }

        public VpcResources(Construct scope)
        {
            Vpc = new Vpc(scope, "LegoEventSignupVPC", new VpcProps
            {
                MaxAzs = 2,
                NatGateways = 1
            });
        }
    }
}