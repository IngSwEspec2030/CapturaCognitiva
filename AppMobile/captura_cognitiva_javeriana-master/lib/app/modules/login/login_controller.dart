import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';
import 'package:capturacognitivajaveriana/app/data/services/auth_service.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:meta/meta.dart';

class LoginController extends GetxController {
  final AuthService authService;
  Prefs prefs = Prefs();
  LoginController({@required this.authService}) : assert(authService != null);

  RxBool _loading = false.obs;

  bool get loading => this._loading.value;
  set loading(bool val) => this._loading.value = val;


  login(String email, String pass) async {
    loading = true;
    var resp = await this.authService.login(email, pass);
    loading = false;

    if (resp != null) {
      print(resp.toString());
      if (resp["response"]) {
        prefs.logged = true;
        prefs.usetToken = resp["userId"];
        
        print("Logged: ${prefs.logged}");
        Get.offAndToNamed(Routes.HOME);
      } else {
        Get.defaultDialog(
          title: "Error de auntenticación.",
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
