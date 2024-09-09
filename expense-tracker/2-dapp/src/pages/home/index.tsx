import { useEffect, useState } from "react";
import { IPortkeyProvider } from "@portkey/provider-types";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";
import Select from "react-select";
import { Id, toast } from "react-toastify";

import "./home.scss";
import { Button } from "@/components/ui/button";
import { CATEGORY_OPTIONS } from "@/lib/constant";
import Modal from "@/components/modal";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { zodResolver } from "@hookform/resolvers/zod";
import { PlusIcon } from "@/components/ui/icons";
import { removeNotification } from "@/lib/utils";
import useExpenseTrackerSmartContract from "@/hooks/useExpenseTrackerSmartContract";
import Card from "@/components/card";
import ExpenseCard from "@/components/expense-card";
import { calculateExpenses } from "@/hooks/useCalculation";

interface IExpenseObject {
  description: string;
  category: string;
  amount: string;
  createdAt: string;
  currency: string;
  expenseId: string;
  updatedAt: string;
}

interface Option {
  value: string;
  label: string;
}

interface PageProps {
  provider: IPortkeyProvider | null;
  currentWalletAddress?: string;
}

const formSchema = z.object({
  description: z.string().min(1, "Description is required"),
  amount: z.string().min(1, "Amount is required"),
  selectedCategory: z
    .object({
      value: z.string(),
      label: z.string(),
    })
    .nullable()
    .refine((val) => val !== null, "Category is required"),
});

const HomePage = ({ provider, currentWalletAddress }: PageProps) => {
  const smartContract = useExpenseTrackerSmartContract(provider);

  const [expenseData, setExpenseData] = useState<IExpenseObject[] | []>([]);
  const [updateId, setUpdateId] = useState<string | null>(null);
  const [deletingId, setDeletingId] = useState<string | null>(null);
  const [selectedCategory, setSelectedCategory] = useState<Option | null>();

  const [isContractInitialized, setIsContractInitialized] =
    useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(true);
  const [formLoading, setFormLoading] = useState<boolean>(false);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const { totalAmount, today, lastMonth } = calculateExpenses(expenseData);

  // Step D - Configure Expense Form
  const form = useForm<z.infer<typeof formSchema>>();

  const handleCloseModal = () => {
    form.reset();
    setIsModalOpen(false);
    setUpdateId(null);
    setSelectedCategory(null);
  };

  // ============ Start Expense Tracker dApp Functions ============ //

  // step 1 - Check if contract is initialized or not
  const checkIsContractInitialized = async () => {

  };

  // step 2 - Initialize the smart contract
  const initializeContract = async () => {};

  // step 3 - Add a New Expense using Smart Contract
  const addNewExpense = async () => {};

  // step 4 - Update the Expense
  const updateExpense = async () => {};

  // step 5 - Delete the Expense
  const deleteExpense = async () => {};

  // step 6 - Handle Submit Form
  const onSubmit = async () => {};

  // step 7 - Fetch All Expenses
  const getExpenseData = async () => {};

  // ============ End Expense Tracker dApp Functions ============ //

  const onEditHandle = (data: IExpenseObject) => {
    // set the UpdateId state
    setUpdateId(data.expenseId);

    // set the default form value for edit Expense
    const categoryObj = {
      label: data.category.charAt(0).toUpperCase() + data.category.slice(1),
      value: data.category,
      amount: data.amount,
    };
    form.setValue("selectedCategory", categoryObj);
    form.setValue("description", data.description);
    form.setValue("amount", data.amount);
    setSelectedCategory(categoryObj);

    // open the form modal
    setIsModalOpen(true);
  };

  // Check whether contract initialized or not
  useEffect(() => {
    smartContract && checkIsContractInitialized();
  }, [smartContract]);

  // Use Effect to Fetch NFTs
  useEffect(() => {
    if (currentWalletAddress) {
      getExpenseData();
    }
  }, [currentWalletAddress]);

  return (
    <div className="home-container">
      <div className="expense-tracker-collection-container">
        <div className="card-header-container">
          <Card totalAmount={totalAmount} title="Total Expenditure" />
          <Card totalAmount={today} title="Today’s Expenditure" />
          <Card totalAmount={lastMonth} title="Last Month’s Expenditure" />
        </div>
        <div className="expense-tracker-collection-head">
          <h2>Activity</h2>
          <div className="button-wrapper">
            <Button
              disabled={!currentWalletAddress}
              onClick={() => setIsModalOpen(true)}
            >
              <PlusIcon />
              Add Expense
            </Button>
          </div>
        </div>
        {currentWalletAddress ? (
          <div className="expense-tracker-collection">
            {expenseData.length > 0 ? (
              expenseData.slice(0, 5).map((data: IExpenseObject, index) => {
                return (
                  <ExpenseCard
                    key={index}
                    data={data}
                    index={index}
                    onEditExpenseHandle={onEditHandle}
                    ondeleteExpenseHandle={deleteExpense}
                    deletingId={deletingId}
                  />
                );
              })
            ) : loading ? (
              <div className="bordered-container">
                <strong>Loading...</strong>
              </div>
            ) : (
              <div className="bordered-container">
                <strong>
                  It's Look like you haven't created any Expense yet
                </strong>
              </div>
            )}
          </div>
        ) : (
          <div className="bordered-container no-wallet">
            <strong>
              Please connect your Portkey Wallet and Add you expenses
            </strong>
          </div>
        )}
        <Modal
          isVisible={isModalOpen}
          title={(updateId ? "Update" : "Add New") + " Expense"}
          onClose={handleCloseModal}
        >
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(onSubmit)}
              className="space-y-8 modal-form"
            >
              <div className="input-group">
                <FormField
                  control={form.control}
                  name="description"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Description</FormLabel>
                      <FormControl>
                        <Input placeholder="Enter Title" {...field} />
                      </FormControl>
                      <FormMessage className="error-message" />
                    </FormItem>
                  )}
                />
              </div>
              <div className="input-group">
                <FormField
                  control={form.control}
                  name="amount"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Amount</FormLabel>
                      <FormControl>
                        <Input placeholder="Enter Amount" {...field} />
                      </FormControl>
                      <FormMessage className="error-message" />
                    </FormItem>
                  )}
                />
              </div>
              <div className="select-container">
                <FormField
                  control={form.control}
                  name="selectedCategory"
                  render={() => (
                    <FormItem>
                      <FormLabel>Select Types</FormLabel>
                      <FormControl>
                        <Controller
                          name="selectedCategory"
                          control={form.control}
                          render={({ field }) => (
                            <Select
                              value={field.value}
                              options={CATEGORY_OPTIONS}
                              //@ts-ignore
                              onChange={(opt: Option) => {
                                field.onChange(opt);
                                setSelectedCategory(opt);
                              }}
                              placeholder="Select Category"
                            />
                          )}
                        />
                      </FormControl>
                      <FormMessage className="error-message" />
                    </FormItem>
                  )}
                />
              </div>
              <div className="button-container">
                <Button
                  type="submit"
                  className="submit-btn"
                  disabled={formLoading}
                >
                  {!!updateId ? "Update" : "Add New"} Expense
                </Button>
              </div>
            </form>
          </Form>
        </Modal>
      </div>
    </div>
  );
};

export default HomePage;
