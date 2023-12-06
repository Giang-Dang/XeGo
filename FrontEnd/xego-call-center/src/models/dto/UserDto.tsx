export default class UserDto {
  userId: string;
  userName: string;
  email: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  address: string;
  roles: string[];

  constructor(
    userId: string,
    userName: string,
    email: string,
    phoneNumber: string,
    firstName: string,
    lastName: string,
    address: string,
    roles: string[],
  ) {
    this.userId = userId; 
    this.userName = userName;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.firstName = firstName;
    this.lastName = lastName;
    this.address = address;
    this.roles = roles;
  }

  static fromJson(json: {
    userId: string;
    userName: string;
    email: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    address: string;
    roles: string[];
  }) {
    return new UserDto(
      json.userId,
      json.userName,
      json.email,
      json.phoneNumber,
      json.firstName,
      json.lastName,
      json.address,
      json.roles
    );
  }
}
