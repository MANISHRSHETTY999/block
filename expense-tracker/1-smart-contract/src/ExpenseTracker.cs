using AElf.Sdk.CSharp;
using Google.Protobuf.WellKnownTypes;
using AElf.Types;
using System.Collections.Generic;

namespace AElf.Contracts.ExpenseTracker
{
    public class ExpenseTrackerContract : ExpenseTrackerContractContainer.ExpenseTrackerContractBase
    {
        // Initialize a new expense entry
        public override Empty InitializeExpense(InitializeExpenseInput input)
        {
            Assert(!State.Expenses[input.Id].Initialized, "Expense already exists.");

            var expense = new Expense
            {
                Id = input.Id,
                MerchantName = input.MerchantName,
                Amount = input.Amount,
                Owner = Context.Sender
            };

            State.Expenses[input.Id] = expense;
            return new Empty();
        }

        // Modify an existing expense entry
        public override Empty ModifyExpense(ModifyExpenseInput input)
        {
            var expense = State.Expenses[input.Id];
            Assert(expense != null && expense.Owner == Context.Sender, "Expense not found or access denied.");

            expense.MerchantName = input.MerchantName;
            expense.Amount = input.Amount;

            State.Expenses[input.Id] = expense;
            return new Empty();
        }

        // Delete an existing expense entry
        public override Empty DeleteExpense(DeleteExpenseInput input)
        {
            var expense = State.Expenses[input.Id];
            Assert(expense != null && expense.Owner == Context.Sender, "Expense not found or access denied.");

            State.Expenses.Remove(input.Id);
            return new Empty();
        }
    }

    public class InitializeExpenseInput
    {
        public long Id { get; set; }
        public string MerchantName { get; set; }
        public long Amount { get; set; }
    }

    public class ModifyExpenseInput
    {
        public long Id { get; set; }
        public string MerchantName { get; set; }
        public long Amount { get; set; }
    }

    public class DeleteExpenseInput
    {
        public long Id { get; set; }
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
