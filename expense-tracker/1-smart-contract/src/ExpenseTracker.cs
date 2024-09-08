using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;

namespace AElf.Contracts.ExpenseTracker
{
    public class ExpenseTracker : ExpenseTrackerContainer.ExpenseTrackerBase
    {
        public override Empty Initialize(Empty input)
        {
            if (State.Initialized.Value)
            {
                return new Empty();
            }
            State.Initialized.Value = true;
            State.Owner.Value = Context.Sender;
            State.ExpenseIds.Value = "";
            State.ExpenseCounter.Value = 0;
            return new Empty();
        }

        public override StringValue AddExpense(ExpenseInput input)
        {
            if (!State.Initialized.Value)
            {
                return new StringValue { Value = "Contract not initialized." };
            }
            var expenseId = (State.ExpenseCounter.Value + 1).ToString();
            State.ExpenseCounter.Value++;
            var timestamp = Context.CurrentBlockTime.Seconds;
            State.Expenses[expenseId] = new Expense
            {
                ExpenseId = expenseId,
                Description = input.Description,
                Category = input.Category,
                Amount = input.Amount, // Now using int64 for amount
                Currency = input.Currency,
                CreatedAt = timestamp,
                UpdatedAt = timestamp,
                Owner = Context.Sender.ToString().Trim('"'),
            };
            State.ExpenseExistence[expenseId] = true;

            var existingExpenseIds = State.ExpenseIds.Value;
            existingExpenseIds += string.IsNullOrEmpty(existingExpenseIds) ? expenseId : $",{expenseId}";
            State.ExpenseIds.Value = existingExpenseIds;

            return new StringValue { Value = expenseId };
        }

        public override Empty UpdateExpense(ExpenseUpdateInput input)
        {
            var expense = State.Expenses[input.ExpenseId];
            if (expense == null)
            {
                return new Empty(); // Handle case if expense doesn't exist
            }
            expense.Description = input.Description ?? expense.Description;
            expense.Category = input.Category ?? expense.Category;
            expense.Amount = input.Amount != 0 ? input.Amount : expense.Amount; // Now using int64 for amount
            expense.Currency = input.Currency ?? expense.Currency;
            expense.UpdatedAt = Context.CurrentBlockTime.Seconds;

            State.Expenses[input.ExpenseId] = expense;
            return new Empty();
        }

        public override Empty DeleteExpense(StringValue input)
        {
            State.Expenses.Remove(input.Value);
            State.ExpenseExistence.Remove(input.Value);

            var existingExpenseIds = State.ExpenseIds.Value.Split(',');
            var newExpenseIds = new List<string>(existingExpenseIds.Length);
            foreach (var expenseId in existingExpenseIds)
            {
                if (expenseId != input.Value)
                {
                    newExpenseIds.Add(expenseId);
                }
            }
            State.ExpenseIds.Value = string.Join(",", newExpenseIds);

            return new Empty();
        }

        public override ExpenseList ListExpenses(StringValue input)
        {
            var owner = input.Value; // Get the owner value from the input
            var expenseList = new ExpenseList();
            var expenseIds = State.ExpenseIds.Value.Split(',');
            foreach (var expenseId in expenseIds)
            {
                var expense = State.Expenses[expenseId];
                if (expense != null && expense.Owner == owner) // Filter expenses by owner
                {
                    expenseList.Expenses.Add(expense);
                }
            }
            return expenseList;
        }

        public override Expense GetExpense(StringValue input)
        {
            var expense = State.Expenses[input.Value];
            if (expense == null)
            {
                return new Expense { ExpenseId = input.Value, Description = "Expense not found." };
            }
            return expense;
        }

        public override BoolValue GetInitialStatus(Empty input)
        {
            return new BoolValue { Value = State.Initialized.Value };
        }
    }
}
