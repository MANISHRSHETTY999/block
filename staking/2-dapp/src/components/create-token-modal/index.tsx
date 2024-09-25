import { useForm } from "react-hook-form";
import { IPortkeyProvider } from "@portkey/provider-types";
import { Id, toast } from "react-toastify";
import * as z from "zod";
import AElf from "aelf-sdk";
import { Buffer } from "buffer";

import Modal from "../modal";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  CustomToast,
  delay,
  removeNotification,
  stakingContractAddress,
} from "@/lib/utils";
import useSmartContract from "@/hooks/useSmartContract";
import { Input } from "../ui/input";
import { InfoIcon } from "@/lib/icons";
import { Button } from "../ui/button";
import useStakingContract from "@/hooks/useStakingConttract";

const formSchema = z.object({
  tokenName: z.string().min(1, "Description is required"),
  symbol: z.string().min(1, "Symbol is required"),
  totalSupply: z.string().min(1, "Description is required"),
});

interface ITokenValidateResult {
  parentChainHeight: string | number;
  signedTx: string;
  merklePath: { merklePathNodes: any };
}

interface ITokenParams {
  tokenName: string;
  symbol: string;
  totalSupply: string;
}

interface IProps {
  isModalOpen: boolean;
  handleCloseModal: () => void;
  formLoading: boolean;
  setFormLoading: (val: boolean) => void;
  currentWalletAddress: string | undefined;
  provider: IPortkeyProvider | null;
}

const CreateTokenodal = ({
  isModalOpen,
  handleCloseModal,
  formLoading,
  setFormLoading,
  currentWalletAddress,
  provider,
}: IProps) => {
  const { mainChainSmartContract, sideChainSmartContract } =
    useSmartContract(provider);
  const stakingContract = useStakingContract(provider);

  const mainchain_from_chain_id = 9992731;
  const sidechain_from_chain_id = 1931928;

  const extraRewardAmount = "5000";

  const tdvw = new AElf(
    new AElf.providers.HttpProvider("https://tdvw-test-node.aelf.io")
  );

  const aelf = new AElf(
    new AElf.providers.HttpProvider("https://aelf-test-node.aelf.io")
  );

  const wallet = AElf.wallet.getWalletByPrivateKey(
    "4e83df2aa7c8552a75961f9ab9f2f06e01e0dca0203e383da5468bbbe2915c97"
  );

  // Step F - Configure Expense Form
  const form = useForm<z.infer<typeof formSchema>>({});

  // Step G - Get CrossChain Contract
  const getCrossChainContract = async () => {};

  // Step H - Get the parent chain height
  const GetParentChainHeight = async () => {};

  // Step I - Fetch the merkle path by transaction Id
  const getMerklePathByTxId = async () => {};

  // Step J - Get Token Contract
  const getTokenContract = async () => {};

  // Step K - Issue Token on SideChain
  const issueTokenOnSideChain = async () => {};

  // Step L - Validate Mainchain Token Create's Transaction
  const validateToken = async () => {};

  // Step N - Create Token on MainChain
  const createTokenOnMainChain = async () => {};

  // Step O - Create a Token on SideChain.
  const createTokenOnSideChain = async () => {};

  // Step P - Transfer TOKEN to Staking Contract
  const transferTokenToStakingContract = async () => {};

  // Step Q - Initializing the staking contract
  const initializedContract = async () => {};

  // Step R - handle Submit of Create Token
  const onSubmit = async () => {};

  return (
    <Modal
      isVisible={isModalOpen}
      title={"Create New Token"}
      onClose={() => {
        form.reset();
        handleCloseModal();
      }}
    >
      <Form {...form}>
        <form
          onSubmit={form.handleSubmit(onSubmit)}
          className="space-y-8 modal-form"
        >
          <div className="input-group">
            <FormField
              control={form.control}
              name="tokenName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Token Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Enter Token Name" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
          <div className="input-group">
            <FormField
              control={form.control}
              name="symbol"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="symbol-label">
                    Symbol <InfoIcon className="info-icon" />
                  </FormLabel>
                  <FormControl>
                    <Input placeholder="Enter Symbol" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          <div className="input-group">
            <FormField
              control={form.control}
              name="totalSupply"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Total Supply</FormLabel>
                  <FormControl>
                    <Input placeholder="Enter Total Supply" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
          <div className="button-container">
            <Button type="submit" className="submit-btn" disabled={formLoading}>
              Create Token
            </Button>
          </div>
        </form>
      </Form>
    </Modal>
  );
};

export default CreateTokenodal;
