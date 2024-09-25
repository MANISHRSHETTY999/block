import { calculateTimeRemaining, dateFormat } from "@/lib/utils";
import { Button } from "../ui/button";
import { useEffect, useState } from "react";

interface IDepositData {
  depositId: string;
  address: string;
  tokenSymbol: string;
  amount: string;
  lockTime: string;
  depositTime: string;
}

const DepositDataCard = ({
  data,
  isWithdrawable,
  isWithdrawing,
  withdrawStake
}: {
  data: IDepositData;
  isWithdrawable?: boolean;
  isWithdrawing:boolean
  withdrawStake:(depositId:string) =>void
}) => {
  const [remianingTime, setRemainingTime] = useState<string>("");

  useEffect(() => {
    setInterval(() => {
      setRemainingTime(
        calculateTimeRemaining(Number(data.depositTime) + Number(data.lockTime))
      );
    }, 1000);
  }, []);
  return (
    <div className="table-data-row">
      <p>{data.amount}</p>
      <p>1.1x</p>
      <p>{isWithdrawable ? Math.round(Number(data.amount) * 1.1) : data.amount}</p>
      <p className="date">{dateFormat(data.depositTime)}</p>
      {!isWithdrawable && <p className="date">{remianingTime}</p>}
      <p className="action">
        <Button disabled={isWithdrawing} onClick={()=>withdrawStake(data.depositId)}>{isWithdrawable ? "Withdraw" : "Emergency Withdraw"}</Button>
      </p>
    </div>
  );
};

export default DepositDataCard;
