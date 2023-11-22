import 'dart:io';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_driver/models/Dto/registration_request_dto.dart';
import 'package:xego_driver/screens/login_screen.dart';
import 'package:xego_driver/screens/vehicle_registration_screen.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/constants.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/widgets/address_input_form_field.dart';
import 'package:xego_driver/widgets/email_input_field.dart';
import 'package:xego_driver/widgets/first_name_input_field.dart';
import 'package:xego_driver/widgets/image_input.dart';
import 'package:xego_driver/widgets/info_section_container.dart';
import 'package:xego_driver/widgets/last_name_input_field.dart';
import 'package:xego_driver/widgets/navigation_rich_text.dart';
import 'package:xego_driver/widgets/password_input_field.dart';
import 'package:xego_driver/widgets/phone_input_field.dart';
import 'package:xego_driver/widgets/reenter_password_input_field.dart';
import 'package:xego_driver/widgets/username_input_field.dart';

class UserRegistrationScreen extends StatefulWidget {
  const UserRegistrationScreen({super.key});

  @override
  State<UserRegistrationScreen> createState() => _UserRegistrationScreenState();
}

class _UserRegistrationScreenState extends State<UserRegistrationScreen> {
  final _formUserRegisterKey = GlobalKey<FormState>();

  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();
  final _reenterPasswordController = TextEditingController();
  final _phoneNumberController = TextEditingController();
  final _emailController = TextEditingController();
  final _firstNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _addressController = TextEditingController();

  File? _selectedImage;

  final _userServices = UserServices();

  bool _isRegistering = false;

  _onNextPressed() async {
    if (!_formUserRegisterKey.currentState!.validate()) {
      return;
    }

    if (mounted) {
      setState(() {
        _isRegistering = true;
      });
    }

    final registrationRequestDto = RegistrationRequestDto(
        email: _emailController.text,
        userName: _usernameController.text,
        phoneNumber: _phoneNumberController.text,
        password: _passwordController.text,
        firstName: _firstNameController.text,
        lastName: _lastNameController.text,
        address: _addressController.text,
        role: Constants.kDefaultRoleValue);

    var response = await _userServices.register(registrationRequestDto);

    if (mounted) {
      setState(() {
        _isRegistering = false;
      });
    }

    if (!response.data['isSuccess']) {
      _showAlertDialog('Registration failed', response.data['message'], () {
        Navigator.pop(context);
      });
      return;
    }

    _showAlertDialog('Registration success', response.data['message'], () {
      if (context.mounted) {
        Navigator.pop(context);
        Navigator.of(context).pushReplacement(MaterialPageRoute(
          builder: (context) => VehicleRegistrationScreen(),
        ));
      }
    });
  }

  _showAlertDialog(String title, String message, void Function() onOkPressed) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(title),
          content: Text(message),
          actions: [
            TextButton(
              child: const Text('OK'),
              onPressed: () {
                onOkPressed();
              },
            ),
          ],
        );
      },
    );
  }

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    _usernameController.dispose();
    _phoneNumberController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _reenterPasswordController.dispose();
    _firstNameController.dispose();
    _lastNameController.dispose();
    _addressController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(Constants.kTopScreenAppTitle),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
        child: Form(
          key: _formUserRegisterKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'User Registration',
                style: Theme.of(context).textTheme.titleLarge!.copyWith(
                      color: KColors.kPrimaryColor,
                      fontSize: 30,
                    ),
              ),
              const Gap(20),
              InfoSectionContainer(
                title: 'Avatar',
                titleColor: KColors.kPrimaryColor.withOpacity(0.8),
                padding: const EdgeInsets.all(14.0),
                innerPadding: const EdgeInsets.all(14.0),
                children: [
                  ImageInput(
                    onPickImage: (image) {
                      _selectedImage = image;
                    },
                  )
                ],
              ),
              InfoSectionContainer(
                title: 'Required Info',
                titleColor: KColors.kPrimaryColor.withOpacity(0.8),
                padding: const EdgeInsets.all(14.0),
                innerPadding: const EdgeInsets.all(14.0),
                children: [
                  UsernameInputField(
                      controller: _usernameController,
                      userServices: _userServices),
                  const Gap(10),
                  PhoneNumberInputField(
                    controller: _phoneNumberController,
                    userServices: _userServices,
                  ),
                  const Gap(10),
                  EmailInputField(
                    controller: _emailController,
                    userServices: _userServices,
                  ),
                  const Gap(10),
                  PasswordInputField(controller: _passwordController),
                  const Gap(10),
                  ReEnterPasswordInputField(
                    controller: _reenterPasswordController,
                    originalPasswordController: _passwordController,
                  ),
                  const Gap(10),
                ],
              ),
              Divider(
                color: KColors.kPrimaryColor.withOpacity(0.8),
              ),
              InfoSectionContainer(
                title: 'About you',
                titleColor: KColors.kPrimaryColor.withOpacity(0.8),
                padding: const EdgeInsets.all(14.0),
                innerPadding: const EdgeInsets.all(14.0),
                children: [
                  FirstNameInputField(controller: _firstNameController),
                  const Gap(10),
                  LastNameInputField(controller: _lastNameController),
                  const Gap(10),
                  AddressInputFormField(controller: _addressController),
                  const Gap(10),
                ],
              ),
              Center(
                child: ElevatedButton(
                  onPressed: () {
                    _onNextPressed();
                  },
                  child: _isRegistering
                      ? const CircularProgressIndicator()
                      : const Text('Next'),
                ),
              ),
              const Gap(15),
              const Center(
                child: NavigationRichText(
                  navigationText: ' to login.',
                  destinationScreen: LoginScreen(),
                ),
              ),
              const Gap(40),
            ],
          ),
        ),
      ),
    );
  }
}
