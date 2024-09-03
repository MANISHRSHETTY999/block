using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.TicTacToe
{
    // The state class is access the blockchain state
    public partial class TicTacToeState : ContractState 
    {
        // A state to check if contract is initialized
        public BoolState Initialized { get; set; }
        public SingletonState<Address> Owner { get; set; }
        public StringState Board { get; set; } // Board state as a concatenated string
        public StringState CurrentPlayer { get; set; } // X or O
        public StringState GameStatus { get; set; } // ongoing, finished, draw
        public StringState Winner { get; set; } // X or O
    }
}