using AElf.Cryptography.ECDSA;
using AElf.Testing.TestBase;

namespace AElf.Contracts.TicTacToe
{
    // The Module class load the context required for unit testing
    public class Module : ContractTestModule<TicTacToe>
    {
        
    }
    
    // The TestBase class inherit ContractTestBase class, it defines Stub classes and gets instances required for unit testing
    public class TestBase : ContractTestBase<Module>
    {
        // The Stub class for unit testing
        internal readonly TicTacToeContainer.TicTacToeStub TicTacToeStub;
        // A key pair that can be used to interact with the contract instance
        private ECKeyPair DefaultKeyPair => Accounts[0].KeyPair;

        public TestBase()
        {
            TicTacToeStub = GetTicTacToeContractStub(DefaultKeyPair);
        }

        private TicTacToeContainer.TicTacToeStub GetTicTacToeContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<TicTacToeContainer.TicTacToeStub>(ContractAddress, senderKeyPair);
        }
    }
    
}