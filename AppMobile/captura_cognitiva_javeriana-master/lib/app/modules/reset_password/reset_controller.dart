import 'package:capturacognitivajaveriana/app/data/services/auth_service.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:meta/meta.dart';

class ResetController extends GetxController {
  
  final AuthService authService;
  ResetController({@required this.authService}) : assert(authService != null);

  RxBool _loading = false.obs;

  bool get loading => this._loading.value;
  set loading(bool val) => this._loading.value = val;


  reset(String email) async {
    loading = true;
    var resp = await this.authService.resetPassword(email);
    loading = false;

    if (resp != null) {
      print(resp.toString());
      if (resp["response"]) {
        Get.offAndToNamed(Routes.LOGIN);
      } else {
        Get.defaultDialog(
          title: "Error de restablecimiento de contraseña.",
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
}
