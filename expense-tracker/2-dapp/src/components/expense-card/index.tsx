import { ClockIcon, DateIcons } from "../ui/icons";
import { calculateTimeRemaining, dateFormat } from "@/lib/utils";
import { Button } from "../ui/button";
import "./expense-card.scss";
import { ExpenseIcon } from "@/lib/icons";

interface IExpenseObject {
  description: string;
  category: string;
  amount: string;
  createdAt: string;
  currency: string;
  expenseId: string;
  updatedAt: string;
}

interface IProps {
  data: IExpenseObject;
  index: number;
  onEditExpenseHandle: (data: IExpenseObject) => void;
  ondeleteExpenseHandle: (deleteTask: IExpenseObject) => void;
  deletingId: string | null;
}

const ExpenseCard = ({
  data,
  index,
  onEditExpenseHandle,
  ondeleteExpenseHandle,
  deletingId,
}: IProps) => {
  return (
    <div className={"expense-card"} key={index}>
      <div className="left-container">
        <div className="icon-box">
        {ExpenseIcon(data.category)}
        </div>
        <div className="info">
          <p className="title">
            <span>$</span>
            {data.amount}
          </p>
          <p className="desc">{data.description}</p>
          <div className="date-and-time-container">
            <div className="date" data-tooltip="Updated Time">
              <ClockIcon />
              <p>{calculateTimeRemaining(data.updatedAt)}</p>
            </div>
            <div className="date" data-tooltip="Created Date and Time">
              <DateIcons />
              <p>{dateFormat(data.createdAt)}</p>
            </div>
          </div>
        </div>
      </div>
      <div className="right-container">
        <div className="tags-wrapper">
          <span>{data.category}</span>
        </div>
        <div className="action-container">
          <Button onClick={() => onEditExpenseHandle(data)}>Edit</Button>
          <Button
            className={`error-btn`}
            disabled={deletingId === data.expenseId}
            onClick={() => ondeleteExpenseHandle(data)}
          >
            {"Remove"}
          </Button>
        </div>
      </div>
    </div>
  );
};

export default ExpenseCard;
