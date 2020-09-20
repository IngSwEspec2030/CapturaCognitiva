import 'package:get/get.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';

class SplashController extends GetxController {
  final Prefs prefs = Prefs();
  RxBool _logged = false.obs;

  bool get logged => this._logged.value;

  set logged(bool val) => this._logged.value;

  @override
  void onReady() {
    logged = prefs.logged;
    if ( prefs.logged ) {
      Get.offAndToNamed(Routes.HOME);
    } else {
      Get.offAndToNamed(Routes.LOGIN);
    }
    super.onReady();
  }
}
