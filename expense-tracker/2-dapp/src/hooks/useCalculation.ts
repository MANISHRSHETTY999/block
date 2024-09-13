import { useMemo } from "react";
import dayjs from "dayjs";
import isBetween from "dayjs/plugin/isBetween"; // Import the isBetween plugin

dayjs.extend(isBetween); // Extend Day.js with the isBetween plugin

interface IExpenseObject {
    description: string;
    category: string;
    amount: string;
    createdAt: string;
    currency: string;
    expenseId: string;
    updatedAt: string;
  }

export const calculateExpenses = (expenseData:IExpenseObject[] | []) => {
  const totalAmount = useMemo(() => {
    return expenseData.length > 0
      ? expenseData.reduce(
          (total: number, expense:IExpenseObject) => total + parseFloat(expense.amount),
          0
        )
      : 0;
  }, [expenseData]);

  const today = useMemo(() => {
    const today = dayjs().startOf("day");
    return expenseData.length > 0
      ? expenseData.reduce((total, expense:IExpenseObject) => {
          const expenseDate = dayjs.unix(Number(expense.createdAt));
          if (expenseDate.isSame(today, "day")) {
            return total + parseFloat(expense.amount);
          }
          console.log("total", total);
          return total;
        }, 0)
      : 0;
  }, [expenseData]);

  const lastMonth = useMemo(() => {
    const startOfLast30Days = dayjs().subtract(30, "days");
    const today = dayjs();
    return expenseData.length > 0
      ? expenseData.reduce((total, expense:IExpenseObject) => {
          const expenseDate = dayjs.unix(Number(expense.createdAt));
          if (expenseDate.isBetween(startOfLast30Days, today)) {
            return total + parseFloat(expense.amount);
          }
          return total;
        }, 0)
      : 0;
  }, [expenseData]);

  return {
    totalAmount,
    today,
    lastMonth,
  };
};
