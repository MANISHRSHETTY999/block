using AElf.Cryptography.ECDSA;
using AElf.Testing.TestBase;

namespace AElf.Contracts.StakingContract
{
    // The Module class load the context required for unit testing
    public class Module : ContractTestModule<StakingContract>
    {
        
    }
    
    // The TestBase class inherit ContractTestBase class, it defines Stub classes and gets instances required for unit testing
    public class TestBase : ContractTestBase<Module>
    {
        // The Stub class for unit testing
        internal readonly StakingContractContainer.StakingContractStub StakingContractStub;
        // A key pair that can be used to interact with the contract instance
        private ECKeyPair DefaultKeyPair => Accounts[0].KeyPair;

        public TestBase()
        {
            StakingContractStub = GetStakingContractContractStub(DefaultKeyPair);
        }

        private StakingContractContainer.StakingContractStub GetStakingContractContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<StakingContractContainer.StakingContractStub>(ContractAddress, senderKeyPair);
        }
    }
    
}