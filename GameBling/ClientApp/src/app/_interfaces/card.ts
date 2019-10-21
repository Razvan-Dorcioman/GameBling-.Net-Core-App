import { User } from "./user";

export class Card {

  id: number;
  cardNumber: number;
  expirationDate: Date;
  cvc: number;
  cardHolderName: string;
  user: User;
}

