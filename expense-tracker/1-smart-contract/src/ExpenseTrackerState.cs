using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.ExpenseTracker
{
    public class ExpenseTrackerState : ContractState
    {
        public BoolState Initialized { get; set; }
        public SingletonState<Address> Owner { get; set; }
        public MappedState<string, Expense> Expenses { get; set; } // Mapping of expense ID to Expense
        public MappedState<string, bool> ExpenseExistence { get; set; } // Mapping to track expense existence
        public StringState ExpenseIds { get; set; } // Concatenated string of expense IDs
        public Int32State ExpenseCounter { get; set; } // Counter for generating unique IDs
    }
}
