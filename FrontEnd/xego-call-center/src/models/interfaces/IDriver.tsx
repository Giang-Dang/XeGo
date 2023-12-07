export default interface IDriver {
  userId: string;
  userName: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  address: string;
  isAssigned: boolean;
  createdBy: string;
  createdDate: Date;
  lastModifiedBy: string;
  lastModifiedDate: Date;
}
