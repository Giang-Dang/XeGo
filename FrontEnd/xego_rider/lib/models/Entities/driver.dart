class Driver {
  final String userId;
  final String userName;
  final String firstName;
  final String lastName;
  final String phoneNumber;
  final String email;
  final String address;
  final bool isAssigned;
  final String createdBy;
  final String createdDate;
  final String lastModifiedBy;
  final String lastModifiedDate;

  Driver(
      {required this.userId,
      required this.userName,
      required this.firstName,
      required this.lastName,
      required this.phoneNumber,
      required this.email,
      required this.address,
      required this.isAssigned,
      required this.createdBy,
      required this.createdDate,
      required this.lastModifiedBy,
      required this.lastModifiedDate});

  Map<String, dynamic> toJson() => {
        'userId': userId,
        'userName': userName,
        'firstName': firstName,
        'lastName': lastName,
        'phoneNumber': phoneNumber,
        'email': email,
        'address': address,
        'isAssigned': isAssigned,
        'createdBy': createdBy,
        'createdDate': createdDate,
        'lastModifiedBy': lastModifiedBy,
        'lastModifiedDate': lastModifiedDate,
      };

  factory Driver.fromJson(Map<String, dynamic> json) {
    return Driver(
      userId: json['userId'],
      userName: json['userName'],
      firstName: json['firstName'],
      lastName: json['lastName'],
      phoneNumber: json['phoneNumber'],
      email: json['email'],
      address: json['address'],
      isAssigned: json['isAssigned'],
      createdBy: json['createdBy'],
      createdDate: json['createdDate'],
      lastModifiedBy: json['lastModifiedBy'],
      lastModifiedDate: json['lastModifiedDate'],
    );
  }
}
