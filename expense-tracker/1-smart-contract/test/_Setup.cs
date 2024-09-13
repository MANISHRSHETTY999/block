using AElf.Cryptography.ECDSA;
using AElf.Testing.TestBase;

namespace AElf.Contracts.ExpenseTracker
{
    // The Module class load the context required for unit testing
    public class Module : ContractTestModule<ExpenseTracker>
    {
        
    }
    
    // The TestBase class inherit ContractTestBase class, it defines Stub classes and gets instances required for unit testing
    public class TestBase : ContractTestBase<Module>
    {
        // The Stub class for unit testing
        internal readonly ExpenseTrackerContainer.ExpenseTrackerStub ExpenseTrackerStub;
        // A key pair that can be used to interact with the contract instance
        private ECKeyPair DefaultKeyPair => Accounts[0].KeyPair;

        public TestBase()
        {
            ExpenseTrackerStub = GetExpenseTrackerContractStub(DefaultKeyPair);
        }

        private ExpenseTrackerContainer.ExpenseTrackerStub GetExpenseTrackerContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<ExpenseTrackerContainer.ExpenseTrackerStub>(ContractAddress, senderKeyPair);
        }
    }
    
}