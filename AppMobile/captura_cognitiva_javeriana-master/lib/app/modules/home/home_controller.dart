import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import 'package:get/get.dart';


class HomeController extends GetxController {

  final Prefs prefs = Prefs();

  logout() {
    prefs.logged = false;

    if(!prefs.logged){
      prefs.usetToken = "";
      Get.offAndToNamed(Routes.SPLASH);
    }
    
  }

  goToCapture() {
    Get.toNamed(Routes.CAPTURA);
  }

}