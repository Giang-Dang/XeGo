class RegistrationRequestDto {
  String email;
  String userName;
  String phoneNumber;
  String password;
  String firstName;
  String lastName;
  String address;
  String role;

  RegistrationRequestDto({
    required this.email,
    required this.userName,
    required this.phoneNumber,
    required this.password,
    required this.firstName,
    required this.lastName,
    required this.address,
    required this.role,
  });

  factory RegistrationRequestDto.fromJson(Map<String, dynamic> json) {
    return RegistrationRequestDto(
      email: json['Email'],
      userName: json['UserName'],
      phoneNumber: json['PhoneNumber'],
      password: json['Password'],
      firstName: json['FirstName'],
      lastName: json['LastName'],
      address: json['Address'],
      role: json['Role'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'Email': email,
      'UserName': userName,
      'PhoneNumber': phoneNumber,
      'Password': password,
      'FirstName': firstName,
      'LastName': lastName,
      'Address': address,
      'Role': role,
    };
  }
}
