using AElf.Sdk.CSharp.State;
using AElf.Types;
using AElf.Contracts.MultiToken;

namespace AElf.Contracts.SinglePoolStaking
{
    // The state class is access the blockchain state
    public class SinglePoolStakingState : ContractState 
    {
        public BoolState Initialized { get; set; }
        public SingletonState<Address> Owner { get; set; }
        public MappedState<string, Deposit> Deposits { get; set; } // Mapping from deposit ID to Deposit
        public MappedState<Address, StringList> UserDeposits { get; set; } // User to deposit IDs
        public Int32State DepositCounter { get; set; }
        public Int64State TotalStakedAmount { get; set; } // New state to track total staked amount

        internal TokenContractContainer.TokenContractReferenceState TokenContract { get; set; }
    }
}