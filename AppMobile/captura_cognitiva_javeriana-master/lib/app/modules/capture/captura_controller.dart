import 'dart:io';
import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';
import 'package:get/get.dart';
import 'package:image_picker/image_picker.dart';
import 'package:meta/meta.dart';
import 'package:flutter/material.dart';
import 'package:capturacognitivajaveriana/app/data/services/captura_service.dart';

class CapturaController extends GetxController {
  final CapturaService capturaService;

  CapturaController({@required this.capturaService})
      : assert(capturaService != null);
  
  final Prefs prefs = Prefs();
  
  RxString _result = "".obs; 
  RxBool _loading = false.obs;
  Rx<File> _foto = Rx<File>(null);

  bool get loading => this._loading.value;
  set loading(bool val) => this._loading.value = val;

  File get foto => this._foto.value;
  set foto(File newFile) => this._foto.value = newFile;

  String get result => this._result.value;
  set result(String result) => this._result.value = result;

  captura(String userToken ,File file) async {
    loading = true;
    var resp = await this.capturaService.analyzerImage(userToken, file);
    loading = false;

    if (resp != null) {
      print(resp.toString());
      if (resp["response"]) {
        result = resp["message"].toString();
      } else {
        Get.defaultDialog(
          title: "Error al capturar la image.",
          content: Container(
            child: Text(
              resp["message"],
            ),
          ),
        );
      }
    } else {
      Get.defaultDialog(
        title: "Error de conexion",
        content: Container(
          child: Text(
            "Error al conectarse a los servidores.",
          ),
        ),
      );
    }
  }

  // Capturar foto
  mostrarFoto() {
    // Mostrar la foto del marbete
    if (foto != null) {
      return Stack(
        children: [
          Center(
            child: ClipRRect(
              borderRadius: BorderRadius.circular(8.0),
              child: Container(
                width: double.infinity,
                height: double.infinity,
                child: Image.file(
                  foto,
                  fit: BoxFit.cover,
                  alignment: Alignment.center,
                ),
              ),
            ),
          ),
          Positioned(
            right: 0.0,
            child: IconButton(
              icon: Icon(Icons.close),
              onPressed: () => foto = null,
            ),
          )
        ],
      );
    }
    return Icon(Icons.edit);
  }

  void tomarFoto() {
    procesarImagen(ImageSource.camera);
  }

  void seleccionarFoto() {
    procesarImagen(ImageSource.gallery);
  }

  procesarImagen(ImageSource origen) async {
    result = "";
    // Procesar foto marbete
    PickedFile pickedFile = await ImagePicker().getImage(
      source: origen,
      imageQuality: 90,
      maxWidth: 400,
    );
    foto = File(pickedFile.path);
    if (foto != null) {
      print("user id: ${prefs.usetToken}");
      print("Foto image: ${foto.path}");
      captura(prefs.usetToken, foto);
    }
  }
}
