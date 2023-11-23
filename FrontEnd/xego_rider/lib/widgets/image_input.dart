import 'dart:io';

import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:xego_rider/settings/kColors.dart';

class ImageInput extends StatefulWidget {
  const ImageInput({
    Key? key,
    required this.onPickImage,
    this.backgroundColor,
    this.icon,
    this.radius,
  }) : super(key: key);

  final void Function(File image) onPickImage;
  final Color? backgroundColor;
  final IconData? icon;
  final double? radius;

  @override
  _ImageInputState createState() => _ImageInputState();
}

class _ImageInputState extends State<ImageInput> {
  File? _selectedImage;

  Future<void> _takePicture() async {
    final imageSource = await _getImageSource();

    if (imageSource == null) return;

    final pickedImage = await ImagePicker().pickImage(
      source: imageSource,
      maxWidth: 600,
    );

    if (pickedImage == null) return;

    setState(() {
      _selectedImage = File(pickedImage.path);
    });

    widget.onPickImage(_selectedImage!);
  }

  Future<ImageSource?> _getImageSource() async {
    return showModalBottomSheet<ImageSource>(
      context: context,
      builder: (context) => Container(
        decoration: BoxDecoration(
          color: KColors.kPrimaryColor.withOpacity(0.2),
          borderRadius: BorderRadius.circular(16.0),
        ),
        padding: const EdgeInsets.fromLTRB(20, 20, 20, 40),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            _buildButton('Camera', Icons.photo_camera, ImageSource.camera),
            const SizedBox(width: 25),
            _buildButton('Gallery', Icons.photo_library, ImageSource.gallery),
          ],
        ),
      ),
    );
  }

  Widget _buildButton(String label, IconData icon, ImageSource source) {
    return ElevatedButton.icon(
      onPressed: () => Navigator.of(context).pop(source),
      label: Text(label),
      icon: Icon(icon),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: CircleAvatar(
        radius: widget.radius ?? 50,
        backgroundColor: Theme.of(context).colorScheme.primary.withOpacity(0.2),
        child: _selectedImage == null
            ? GestureDetector(
                onTap: _takePicture,
                child: CircleAvatar(
                  radius: widget.radius ?? 50,
                  backgroundColor:
                      widget.backgroundColor ?? KColors.kPrimaryColor,
                  backgroundImage: _selectedImage != null
                      ? FileImage(_selectedImage!)
                      : null,
                  child: _selectedImage == null
                      ? Icon(widget.icon ?? Icons.person_outline, size: 50)
                      : null,
                ),
              )
            : GestureDetector(
                onTap: _takePicture,
                child: CircleAvatar(
                  radius: 80,
                  backgroundImage: FileImage(_selectedImage!),
                ),
              ),
      ),
    );
  }
}
