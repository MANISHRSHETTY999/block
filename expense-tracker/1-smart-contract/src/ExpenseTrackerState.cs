using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.ExpenseTracker
{
    public partial class ExpenseTrackerContractState : ContractState
    {
        public MappedState<long, Expense> Expenses { get; set; }
    }

    public class Expense
    {
        public long Id { get; set; }
        public Address Owner { get; set; }
        public string MerchantName { get; set; }
        public long Amount { get; set; }
        public bool Initialized { get; set; } = true;
    }
}
