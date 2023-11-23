class UserDto {
  final String userId;
  final String userName;
  final String email;
  final String phoneNumber;
  final String firstName;
  final String lastName;
  final String address;

  UserDto({
    required this.userId,
    required this.userName,
    required this.email,
    required this.phoneNumber,
    required this.firstName,
    required this.lastName,
    required this.address,
  });

  factory UserDto.fromJson(Map<String, dynamic> json) {
    return UserDto(
      userId: json['userId'],
      userName: json['userName'],
      email: json['email'],
      phoneNumber: json['phoneNumber'],
      firstName: json['firstName'],
      lastName: json['lastName'],
      address: json['address'],
    );
  }
}
