import { DollorIcon } from "@/lib/icons";
import "./card.scss";

const Card = ({ totalAmount,title }: { totalAmount: number | undefined,title:string }) => {
  return (
    <div className="total-expense-card">
      <span className="bubbles" />
      <span className="bubbles" />
      <span className="bubbles" />
      <DollorIcon className="dollor-icn" />
      <p>{title}</p>
      <div className="expense-value">
        <p>$</p>
        <h2>{totalAmount}</h2>
      </div>
    </div>
  );
};

export default Card;
