import { IPortkeyProvider, IChain } from "@portkey/provider-types";
import { useEffect, useState } from "react";

const useSmartContract = (provider: IPortkeyProvider | null) => {
  const [smartContract, setSmartContract] =
    useState<ReturnType<IChain["getContract"]>>();

  //Step A - Function to fetch a smart contract based on deployed wallet address
  const fetchContract = async () => {};

  // Step B -  Effect hook to initialize and fetch the smart contract when the provider changes
  useEffect(() => {

  }, []); // Dependency array ensures this runs when the provider changes

  return smartContract;
};

export default useSmartContract;
