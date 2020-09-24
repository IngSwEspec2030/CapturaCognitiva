import 'dart:io';
import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';
import 'package:capturacognitivajaveriana/app/data/services/captura_service.dart';
import 'package:flutter/material.dart';
import 'package:flutter_native_image/flutter_native_image.dart';
import 'package:get/route_manager.dart';
import 'package:image_picker/image_picker.dart';

class CapturaPage extends StatefulWidget {
  @override
  _CapturaPageState createState() => _CapturaPageState();
}

class _CapturaPageState extends State<CapturaPage> {
  final CapturaService capturaService = CapturaService();
  final Prefs prefs = Prefs();

  String result = "";
  bool loading = false;
  File _foto;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Capturar imagen")),
      body: Container(
        constraints: BoxConstraints.expand(),
        child: SingleChildScrollView(
          padding: EdgeInsets.all(12.0),
          child: Column(
            mainAxisSize: MainAxisSize.max,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Container(
                width: double.infinity,
                decoration: BoxDecoration(
                    border: Border.all(width: 2.0, color: Colors.blueGrey[200]),
                    borderRadius: BorderRadius.circular(12.0)),
                child: AspectRatio(
                  aspectRatio: 1280 / 1199,
                  child: _foto != null
                      ? _mostrarFoto()
                      : Icon(
                          Icons.camera_alt,
                          size: 40.0,
                          color: Colors.blueGrey[200],
                        ),
                ),
              ),
              Container(
                width: double.infinity,
                child: FlatButton(
                    color: Colors.blueAccent,
                    child: Text(
                      "TOMAR FOTO",
                      style: TextStyle(
                          fontWeight: FontWeight.bold, color: Colors.white),
                    ),
                    onPressed: _tomarFoto),
              ),
              Text("Resultado"),
              loading ? CircularProgressIndicator() : SizedBox.shrink(),
              Text(result.toString()),
            ],
          ),
        ),
      ),
    );
  }

  // Capturar foto
  _mostrarFoto() {
    // Mostrar la foto del marbete
    if (_foto != null) {
      return Stack(
        children: [
          Center(
            child: ClipRRect(
              borderRadius: BorderRadius.circular(8.0),
              child: Container(
                width: double.infinity,
                height: double.infinity,
                child: Image.file(
                  _foto,
                  fit: BoxFit.fill,
                  height: 1280,
                  width: 1199,
                  alignment: Alignment.center,
                ),
              ),
            ),
          ),
          Positioned(
            right: 0.0,
            child: IconButton(
              icon: Icon(Icons.close),
              onPressed: () {
                setState(() {
                  _foto = null;
                });
              },
            ),
          )
        ],
      );
    }
    return Icon(Icons.edit);
  }

  void _tomarFoto() {
    _procesarImagen(ImageSource.camera);
  }

  _procesarImagen(ImageSource origen) async {
    setState(() {
      result = "";
    });
    // Procesar foto marbete
    PickedFile pickedFile = await ImagePicker().getImage(
      preferredCameraDevice: CameraDevice.rear,
      source: origen,
      maxHeight: 1280,
      maxWidth: 1199,
      imageQuality: 10,
    );

    // File compressedImage = await FlutterNativeImage.compressImage(
    //   pickedFile.path,
    //   percentage: 100,
    //   quality: 20,
    // );

    setState(() {
      _foto = File(pickedFile.path);
    });
    if (_foto != null) {
      print("user id: ${prefs.usetToken}");
      print("Foto image: ${_foto.path}");
      captura(prefs.usetToken, _foto);
    }
  }

  captura(String userToken, File file) async {
    setState(() => loading = true);
    capturaService.analyzerImage(userToken, file).then((resp) {
      print(resp.toString());
      setState(() => loading = false);
      if (resp["response"]) {
        setState(() => result = resp["message"].toString());
      } else {
        setState(() => result = resp["message"].toString());
        Get.defaultDialog(
          title: "Error al analizar la imagen.",
          content: Container(
            child: Text(
              resp["message"],
            ),
          ),
        );
      }
    }).catchError((error) {
      Get.defaultDialog(
        title: "Error al analizar la imagen.",
        content: Container(
          child: Text(
            error.toString(),
          ),
        ),
      );
    });
  }
}
