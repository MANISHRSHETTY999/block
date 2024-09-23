using Google.Protobuf.WellKnownTypes;
using AElf.Types;
using System.Collections.Generic;
using AElf.Contracts.MultiToken;


namespace AElf.Contracts.StakingContract
{
    public class StakingContract : StakingContractContainer.StakingContractBase
    {
        private const int RewardRate = 10; // 10% reward

        public override Empty Initialize(InitializeInput input)
        {
            if (State.Initialized.Value)
                return new Empty();

            State.Initialized.Value = true;
            State.Owner.Value = Context.Sender;
            State.DepositCounter.Value = 0;
            State.TotalStakedAmount.Value = 0; // Initialize total staked amount

            State.TokenContract.Value = input.TokenContractAddress;

            return new Empty();
        }

        public override StringValue Deposit(DepositInput input)
        {
            var depositId = (State.DepositCounter.Value + 1).ToString();
            State.DepositCounter.Value++;

            var deposit = new Deposit
            {
                DepositId = depositId,
                Address = Context.Sender.ToString(),
                TokenSymbol = input.TokenSymbol,
                Amount = input.Amount,
                LockTime = input.LockTime,
                DepositTime = Context.CurrentBlockTime.Seconds
            };

            State.Deposits[depositId] = deposit;

            var userDeposits = State.UserDeposits[Context.Sender] ?? new StringList();
            userDeposits.Values.Add(depositId);

            State.UserDeposits[Context.Sender] = userDeposits;

            State.TotalStakedAmount.Value += input.Amount; // Update total staked amount

            return new StringValue { Value = depositId };
        }

        public override Empty Withdraw(WithdrawInput input)
        {
            var deposit = State.Deposits[input.DepositId];
            Assert(deposit != null, "Deposit not found.");
            Assert(deposit.Address == Context.Sender.ToString(), "Unauthorized.");
            Assert(Context.CurrentBlockTime.Seconds >= deposit.DepositTime + deposit.LockTime, "Lock period not over.");

            var reward = CalculateReward(deposit.Amount);
            
            TransferFromContract(deposit.TokenSymbol, Context.Sender, deposit.Amount + reward);

            State.TotalStakedAmount.Value -= deposit.Amount; // Update total staked amount

            RemoveDeposit(deposit.DepositId);
            return new Empty();
        }

        public override Empty ForceWithdraw(StringValue input)
        {
            var deposit = State.Deposits[input.Value];
            Assert(deposit != null, "Deposit not found.");
            Assert(deposit.Address == Context.Sender.ToString(), "Unauthorized.");

            TransferFromContract(deposit.TokenSymbol, Context.Sender, deposit.Amount);

            State.TotalStakedAmount.Value -= deposit.Amount; // Update total staked amount

            RemoveDeposit(deposit.DepositId);
            return new Empty();
        }

        public override Int64Value GetReward(StringValue input)
        {
            var deposit = State.Deposits[input.Value];
            Assert(deposit != null, "Deposit not found.");
            return new Int64Value { Value = CalculateReward(deposit.Amount) };
        }

        public override DepositList GetDeposits(StringValue input)
        {
            var deposits = State.UserDeposits[Address.FromBase58(input.Value)];
            var depositList = new DepositList();
    
            foreach (var depositId in deposits.Values)
            {
                var deposit = State.Deposits[depositId];
                if (deposit != null)
                {
                    depositList.Deposits.Add(deposit);
                }
            }

            return depositList;
        }

        // New function to check if initialized
        public override BoolValue IfInitialized(Empty input)
        {
            return new BoolValue { Value = State.Initialized.Value };
        }

        // New function to get the total staked amount
        public override Int64Value GetTotalStakedAmount(Empty input)
        {
            return new Int64Value { Value = State.TotalStakedAmount.Value };
        }

        private long CalculateReward(long amount)
        {
            return (amount * RewardRate) / 100;
        }

        private void TransferFromContract(string symbol, Address to, long amount)
        {
            var virtualAddressHash = GetVirtualAddressHash(Context.Self, symbol);

            State.TokenContract.TransferFrom.Send(
                new TransferFromInput
                {
                    Symbol = symbol,
                    Amount = amount,
                    From = Context.Self,
                    Memo = "Transfer from Staking Contract",
                    To = to
                });
        }

        private static Hash GetVirtualAddressHash(Address contractAddress, string symbol)
        {
            return HashHelper.ConcatAndCompute(HashHelper.ComputeFrom(contractAddress), HashHelper.ComputeFrom(symbol));
        }

        private Address GetVirtualAddress(Hash virtualAddressHash)
        {
            return Context.ConvertVirtualAddressToContractAddress(virtualAddressHash);
        }

        private void RemoveDeposit(string depositId)
        {
            var deposit = State.Deposits[depositId];
            State.Deposits.Remove(depositId);

            var userDeposits = State.UserDeposits[Address.FromBase58(deposit.Address)];
            userDeposits.Values.Remove(depositId);

            State.UserDeposits[Address.FromBase58(deposit.Address)] = userDeposits;
        }
    }
}