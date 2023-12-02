import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_rider/models/Dto/login_request_dto.dart';
import 'package:xego_rider/screens/main_tabs_screen.dart';
import 'package:xego_rider/screens/rider_registration_screen.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/services/vehicle_services.dart';
import 'package:xego_rider/settings/app_constants.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/navigation_rich_text.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _phoneNumberController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();

  final UserServices _userServices = UserServices();
  final VehicleServices _vehicleServices = VehicleServices();

  late bool _isPasswordObscured;
  late bool _isLogining;
  bool _isLoginFailed = false;

  Future<void> _login(BuildContext context) async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    _setLogining(true);

    final loginRequestDto = LoginRequestDto(
      phoneNumber: _phoneNumberController.text,
      password: _passwordController.text,
    );

    final isLoginSuccess = await _userServices.login(loginRequestDto);

    _setLogining(false);

    if (isLoginSuccess) {
      final isUpdateRiderTypeSuccess =
          await _userServices.updateRiderType(UserServices.userDto!.userId);
      final isDriverAssigned =
          await _vehicleServices.isDriverAssigned(UserServices.userDto!.userId);
      _setLoginFailed(false);
      _navigateToNextScreen(context);
    } else {
      _setLoginFailed(true);
    }
  }

  void _setLogining(bool isLogining) {
    if (mounted) {
      setState(() {
        _isLogining = isLogining;
      });
    }
  }

  void _setLoginFailed(bool isLoginFailed) {
    setState(() {
      _isLoginFailed = isLoginFailed;
    });
  }

  void _navigateToNextScreen(BuildContext context) {
    if (context.mounted) {
      Navigator.of(context).pushReplacement(
        MaterialPageRoute(
          builder: (context) => const MainTabsScreen(),
        ),
      );
    }
  }

  @override
  void initState() {
    super.initState();
    _isPasswordObscured = true;
    _isLogining = false;
  }

  @override
  void dispose() {
    _phoneNumberController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(AppConstants.kTopScreenAppTitle),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.fromLTRB(35, 25, 40, 0),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              const Gap(30),
              Text(
                'Log In',
                style: Theme.of(context).textTheme.titleLarge!.copyWith(
                      color: KColors.kPrimaryColor,
                      fontSize: 40,
                    ),
              ),
              const Gap(80),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  const Icon(
                    Icons.phone_iphone,
                    size: 35,
                  ),
                  const SizedBox(width: 15),
                  Expanded(
                    child: TextFormField(
                      decoration: const InputDecoration(
                        label: Text('Enter your phone number.'),
                      ),
                      controller: _phoneNumberController,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Please enter your phone number.';
                        }
                        if (!_userServices.isValidPhoneNumber(value)) {
                          return 'Please enter valid phone number';
                        }
                        return null;
                      },
                      keyboardType: TextInputType.phone,
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 10),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  const Icon(
                    Icons.lock,
                    size: 35,
                  ),
                  const SizedBox(width: 15),
                  Expanded(
                    child: TextFormField(
                      obscureText: _isPasswordObscured,
                      decoration: InputDecoration(
                        label: const Text('Enter your password.'),
                        suffixIcon: IconButton(
                          icon: _isPasswordObscured
                              ? const Icon(Icons.visibility)
                              : const Icon(Icons.visibility_off),
                          onPressed: () {
                            setState(() {
                              _isPasswordObscured = !_isPasswordObscured;
                            });
                          },
                        ),
                      ),
                      controller: _passwordController,
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Please enter your password.';
                        }
                        return null;
                      },
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 20),
              if (_isLoginFailed)
                Text(
                  'Login Failed. Please check your phone number and password.',
                  style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                        color: KColors.kDanger,
                      ),
                  textAlign: TextAlign.center,
                ),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: () {
                  _login(context);
                },
                child: _isLogining
                    ? const CircularProgressIndicator()
                    : const Text('Login'),
              ),
              const SizedBox(height: 15),
              const NavigationRichText(
                navigationText: ' to register.',
                destinationScreen: RiderRegistrationScreen(),
              ),
              const Gap(80),
              Transform.scale(
                scale: 1.5,
                child: Image.asset('assets/images/car_loading.gif'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
