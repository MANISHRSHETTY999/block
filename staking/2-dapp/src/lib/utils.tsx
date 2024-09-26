import { type ClassValue, clsx } from "clsx";
import { toast } from "react-toastify";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const removeNotification = (id: number | string, time?: number) => {
  setTimeout(() => toast.done(id), time || 3000);
};

export const delay = (ms: number) =>
  new Promise((resolve) => setTimeout(resolve, ms));

export const CustomToast = ({ title, message }: any) => (
  <div>
    <h4>{title}</h4>
    <p>{message}</p>
  </div>
);

export const calculateTimeRemaining = (endTime: number) => {
  const nowTime = Math.floor(new Date().getTime() / 1000);
  const difference = endTime - nowTime;

  const hours = Math.floor(difference / 3600);
  const minutes = Math.floor((difference % 3600) / 60);
  const seconds = difference % 60;

  return (hours > 0 ? hours : "00") + ":" + (minutes > 0 ? minutes : "00") + ":" + (seconds > 0 ? seconds : "00")
};

export const dateFormat = (date: string) => {
  const unixTimestamp = Number(date);
  const formattedDate = new Date(unixTimestamp * 1000).toLocaleString();
  return formattedDate;
};

// Step Y - Staking contract address
export const stakingContractAddress = "your_deployed_staking_contract_address"
