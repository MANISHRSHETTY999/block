using AElf.Cryptography.ECDSA;
using AElf.Testing.TestBase;

namespace AElf.Contracts.SinglePoolStaking
{
    // The Module class load the context required for unit testing
    public class Module : ContractTestModule<SinglePoolStaking>
    {
        
    }
    
    // The TestBase class inherit ContractTestBase class, it defines Stub classes and gets instances required for unit testing
    public class TestBase : ContractTestBase<Module>
    {
        // The Stub class for unit testing
        internal readonly SinglePoolStakingContainer.SinglePoolStakingStub SinglePoolStakingStub;
        // A key pair that can be used to interact with the contract instance
        private ECKeyPair DefaultKeyPair => Accounts[0].KeyPair;

        public TestBase()
        {
            SinglePoolStakingStub = GetSinglePoolStakingContractStub(DefaultKeyPair);
        }

        private SinglePoolStakingContainer.SinglePoolStakingStub GetSinglePoolStakingContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<SinglePoolStakingContainer.SinglePoolStakingStub>(ContractAddress, senderKeyPair);
        }
    }
    
}