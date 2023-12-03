export default class UserDto {
  userId: string;
  userName: string;
  email: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  address: string;

  constructor(
    userId: string,
    userName: string,
    email: string,
    phoneNumber: string,
    firstName: string,
    lastName: string,
    address: string
  ) {
    this.userId = userId;
    this.userName = userName;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.firstName = firstName;
    this.lastName = lastName;
    this.address = address;
  }

  static fromJson(json: {
    userId: string;
    userName: string;
    email: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    address: string;
  }) {
    return new UserDto(
      json.userId,
      json.userName,
      json.email,
      json.phoneNumber,
      json.firstName,
      json.lastName,
      json.address
    );
  }
}
